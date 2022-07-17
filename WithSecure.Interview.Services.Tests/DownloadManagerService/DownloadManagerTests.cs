using FluentAssertions;
using WithSecure.Interview.Services.DownloadManagerService;
using WithSecure.Interview.Services.Tests.DownloadManagerService.Helper;

namespace WithSecure.Interview.Services.Tests.DownloadManagerService
{
    public class DownloadManagerTests
    {
        private string url = "http://localhost";

        [Fact]
        public async Task GetByteAsync_WhenContentLengthIsZero_ThenShouldThrowRangeException()
        {
            //Arrange
            HttpResponseMessage mockResponse = new();
            mockResponse.Content.Headers.ContentLength = 0;

            var mockHttpClient = HttpClientHelper
                                .CreateMockHandler(mockResponse)
                                .CreateMockHttClient();

            var downloadManager = new DownloadManager(mockHttpClient, url);
            
            //Act
            var action = async () => await downloadManager.GetByteArrayAsync().ConfigureAwait(false);

            //Assert
            await action.Should().ThrowAsync<ArgumentOutOfRangeException>().ConfigureAwait(false);
        }

        [Fact]
        public async Task GetByteAsync_WhenContentLengthIsGreaterThan250Mb_ThenShouldThrowRangeException()
        {
            //Arrange
            HttpResponseMessage mockResponse = new();
            mockResponse.Content.Headers.ContentLength = 250_000_001;

            var mockHttpClient = HttpClientHelper
                                .CreateMockHandler(mockResponse)
                                .CreateMockHttClient();

            var downloadManager = new DownloadManager(mockHttpClient, url);

            //Act
            var action = async () => await downloadManager.GetByteArrayAsync().ConfigureAwait(false);

            //Assert
            await action.Should().ThrowAsync<ArgumentOutOfRangeException>().ConfigureAwait(false);
        }

        [Fact]
        public async Task GetBytesArrayAsync_WhenUrlIsNotValid_ThenThrowError()
        {
            //Arrange
            var downloadManager = new DownloadManager("https://foo");
            //Act
            var action = async () => await downloadManager.GetByteArrayAsync();
            //Assert
            await action.Should().ThrowAsync<HttpRequestException>();
        }
    }
}