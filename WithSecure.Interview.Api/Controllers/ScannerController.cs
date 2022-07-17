using WithSecure.Interview.Services.DownloadManagerServiece;
using WithSecure.Interview.Api.Dtos.VirusChecker;
using WithSecure.Interview.Api.Dtos.Scanner;
using WithSecure.Interview.Common.Helper;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Text.Json;
using System.Text;
using System.IO.Compression;

namespace WithSecure.Interview.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScannerController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> ScanFile(ScannerRequestDto fileDto)
        {
            var url = fileDto.UrlAddress;
            if (!string.IsNullOrWhiteSpace(url))
            {
                var downloadManager = new DownloadManager(url);
                var fileInByteArray = await downloadManager.GetByteArrayAsync();
                var virusCheckingResult = await CheckFileForVirus(fileInByteArray);
                var hashedValue = SecurityHelper.CalculateSHA1(fileInByteArray);
                return Ok(new ScannerResponseDto()
                {
                        result= virusCheckingResult,
                        Sha1= hashedValue
                });
            }
            else
            {
                throw new Exception("Url can not be null!");
            }
        }

        private async Task<string> CheckFileForVirus(byte[] fileInByteArray)
        {           
            var restClient = new RestClient("https://localhost:7198/");
            var request = new RestRequest($"api/VirusChecker", Method.Post);
            request.RequestFormat = DataFormat.Json;
            request.AlwaysMultipartFormData = true;
            request.AddHeader("Content-Type", "multipart/form-data");

            
            if (isLargefile(fileInByteArray))
            {
                var chunks = fileInByteArray.Chunk(10_000_000);
                AddChunksToRequest(request, chunks);
            }
            else {
                request.AddFile("file", fileInByteArray, "file");
            }

            var response = await restClient.PostAsync<VirusCheckerResponseDto>(request);
            ArgumentNullException.ThrowIfNull(response);
            return response.Result;
        }

        private static void AddChunksToRequest(RestRequest request, IEnumerable<byte[]> chunks)
        {
            var index = 0;
            foreach (var chunk in chunks)
            {
                request.AddFile("file", chunk, $"{index}");
                index++;
            }
        }

        private bool isLargefile(byte[] fileInByteArray)
        {
            return fileInByteArray.Length > 10_000_000 ? true : false ;
        }
    }
}
