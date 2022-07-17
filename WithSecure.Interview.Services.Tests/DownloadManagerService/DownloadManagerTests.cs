using FluentAssertions;
using WithSecure.Interview.Services.DownloadManagerService;

namespace WithSecure.Interview.Services.Tests.DownloadManagerService
{
    public class DownloadManagerTests
    {
        [Fact]
        public async Task GetBytesArrayAsync_WhenUrlIsNotValid_ThenThrowError()
        {
            //Arrange
            var downloadManager = new DownloadManager("https://foo");
            //Act
            Func<Task> action = async () => await downloadManager.GetByteArrayAsync();
            //Assert
            await action.Should().ThrowAsync<HttpRequestException>();
        }

        //[Fact]
        //public async Task Chunk_WhenContentLengthIsZero_ThenChunksCountShouldBeZero()
        //{
        //    //Arrange
        //    HttpResponseMessage mockResponse = new();
        //    mockResponse.Content.Headers.ContentLength = 0;


        //    var mockHttpClient = HttpClientHelper
        //                        .CreateMockHandler(mockResponse)
        //                        .CreateMockHttClient();

        //    //Act
        //    var chunkManager = new ChunkManager(mockHttpClient, url);
        //    var chunks = await chunkManager.Chunk();

        //    //Assert
        //    chunks.Count.Should().Be(0);
        //}
    }
}