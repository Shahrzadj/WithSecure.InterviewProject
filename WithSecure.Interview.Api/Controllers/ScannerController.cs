using WithSecure.Interview.Services.DownloadManagerServiece;
using WithSecure.Interview.Api.Dtos.VirusChecker;
using WithSecure.Interview.Api.Dtos.Scanner;
using WithSecure.Interview.Common.Helper;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Text.Json;

namespace WithSecure.Interview.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScannerController : ControllerBase
    {
        private readonly RestClient _client;
        public ScannerController()
        {
            _client = new RestClient("https://localhost:7198/");
        }
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
            string jsonString = JsonSerializer.Serialize(fileInByteArray);
            var request = new RestRequest($"api/VirusChecker").AddJsonBody(new VirusCheckerRequestDto { FileInByteArray = jsonString });
            var response = await _client.PostAsync<VirusCheckerResponseDto>(request);
            return response.Result;

         
        }
    }
}
