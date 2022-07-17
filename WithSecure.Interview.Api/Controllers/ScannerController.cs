using WithSecure.Interview.Services.DownloadManagerService;
using WithSecure.Interview.Api.Dtos.VirusChecker;
using WithSecure.Interview.Api.Dtos.Scanner;
using WithSecure.Interview.Api.Helper;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace WithSecure.Interview.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScannerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ScannerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> ScanFile(ScannerRequestDto fileDto)
        {
            var url = fileDto.UrlAddress;
            try
            {
                if (!string.IsNullOrWhiteSpace(url))
                {
                    var downloadManager = new DownloadManager(url);
                    var fileInByteArray = await downloadManager.GetByteArrayAsync();
                    var virusCheckingResult = await CheckFileForVirus(fileInByteArray);
                    var hashedValue = SecurityHelper.CalculateSHA1(fileInByteArray);
                    return Ok(new ScannerResponseDto()
                    {
                        result = virusCheckingResult,
                        Sha1 = hashedValue
                    });
                }
                else
                {
                    throw new Exception("Url can not be null!");
                }
            }
            catch(Exception ex)
            {
                throw new Exception("An Error Occured "+ex.Message);
            }

        }

        private async Task<string> CheckFileForVirus(byte[] fileInByteArray)
        {
            try
            {
                var chunkSize = 10_000_000; //10 MB

                var baseApiUrl = _configuration.GetSection("ApiConfiguration:BaseAddress").Value;
                var restClient = new RestClient(baseApiUrl);
                var request = new RestRequest($"api/VirusChecker", Method.Post);
                request.RequestFormat = DataFormat.Json;
                request.AlwaysMultipartFormData = true;
                request.AddHeader("Content-Type", "multipart/form-data");


                if (isLargefile(fileInByteArray, chunkSize))
                {
                    var chunks = fileInByteArray.Chunk(chunkSize);
                    AddChunksToRequest(request, chunks);
                }
                else
                {
                    request.AddFile("file", fileInByteArray, "file");
                }

                var response = await restClient.PostAsync<VirusCheckerResponseDto>(request);
                ArgumentNullException.ThrowIfNull(response);
                return response.Result;
            }
            catch (Exception ex)
            {
                throw new Exception("An Error Occured " + ex.Message);
            }
           
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

        private bool isLargefile(byte[] fileInByteArray, int chunkSize)
        {
            return fileInByteArray.Length > chunkSize ? true : false ;
        }
    }
}
