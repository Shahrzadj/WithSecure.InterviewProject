using WithSecure.Interview.Services.DownloadManagerService.Http;
using WithSecure.Interview.Services.Tests.DownloadManagerService.Helper;
using FluentAssertions;
using System.Net;

namespace WithSecure.Interview.Services.Tests.DownloadManagerService
{
    public class HttpClientServicesTests
    {
        private readonly HttpClientServices _httpClientServices;
        private string url = "http://localhost";

        public HttpClientServicesTests()
        {
            _httpClientServices = new HttpClientServices();
        }

        [Fact]
        public async Task Chunk_WhenRequestWasNotSuccessfull_ThenShouldThrowException()
        {
            //Arrange
            HttpResponseMessage mockResponse = new();
            mockResponse.StatusCode = HttpStatusCode.BadRequest;


            var mockHttpClient = HttpClientHelper
                                .CreateMockHandler(mockResponse)
                                .CreateMockHttClient();

            //Act
            var action = async () => await _httpClientServices.GetContentLength(mockHttpClient, url).ConfigureAwait(false);

            //Assert
            await action.Should().ThrowAsync<HttpRequestException>();
        }

        [Fact]
        public async Task Chunk_WhenContentLengthIsNull_ThenShouldThrowException()
        {
            //Arrange
            HttpResponseMessage mockResponse = new();
            mockResponse.Content.Headers.ContentLength = null;


            var mockHttpClient = HttpClientHelper
                                .CreateMockHandler(mockResponse)
                                .CreateMockHttClient();

            //Act
            var action = async () => await _httpClientServices.GetContentLength(mockHttpClient, url).ConfigureAwait(false);

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Chunk_WhenContentLengthIsZero_ThenShouldThrowException()
        {
            //Arrange
            HttpResponseMessage mockResponse = new();
            mockResponse.Content.Headers.ContentLength = 0;


            var mockHttpClient = HttpClientHelper
                                .CreateMockHandler(mockResponse)
                                .CreateMockHttClient();

            //Act
            var action = async () => await _httpClientServices.GetContentLength(mockHttpClient, url).ConfigureAwait(false);

            //Assert
            await action.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }

        [Fact]
        public async Task Chunk_WhenContentLengthIsGreaterThan250Mb_ThenShouldThrowException()
        {
            //Arrange
            HttpResponseMessage mockResponse = new();
            mockResponse.Content.Headers.ContentLength = 250_000_001;


            var mockHttpClient = HttpClientHelper
                                .CreateMockHandler(mockResponse)
                                .CreateMockHttClient();

            //Act
            var action = async () => await _httpClientServices.GetContentLength(mockHttpClient, url).ConfigureAwait(false);

            //Assert
            await action.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }
    }
}
