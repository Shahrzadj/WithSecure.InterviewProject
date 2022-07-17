using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WithSecure.Interview.Services.DownloadManagerService.Helper;
using WithSecure.Interview.Services.Tests.DownloadManagerService.Helper;

namespace WithSecure.Interview.Services.Tests.DownloadManagerService
{
    public class HttpClientServicesTests
    {
        private string url = "http://localhost";

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
            var action = async () => await HttpClientServices.GetContentLength(mockHttpClient, url).ConfigureAwait(false);

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
            var action = async () => await HttpClientServices.GetContentLength(mockHttpClient, url).ConfigureAwait(false);

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
            var action = async () => await HttpClientServices.GetContentLength(mockHttpClient, url).ConfigureAwait(false);

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
            var action = async () => await HttpClientServices.GetContentLength(mockHttpClient, url).ConfigureAwait(false);

            //Assert
            await action.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }
    }
}
