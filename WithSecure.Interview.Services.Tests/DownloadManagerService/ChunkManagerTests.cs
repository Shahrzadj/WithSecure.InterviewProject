using WithSecure.Interview.Services.DownloadManagerServiece.Chunker;
using FluentAssertions;
using WithSecure.Interview.Services.Tests.DownloadManagerService.Helper;
using System.Net;

namespace WithSecure.Interview.Services.Tests.DownloadManagerService
{
    public class ChunkManagerTests
    {
        private string url = "http://localhost";

        [Fact]
        public async Task Chunk_WhenContentLengthIsNull_ThenThrowErrors()
        {
            //Arrange
            HttpResponseMessage mockResponse = new();
            mockResponse.Content.Headers.ContentLength = null;

            var mockHttpClient = HttpClientHelper
                                .CreateMockHandler(mockResponse)
                                .CreateMockHttClient();

            var chunkManager = new ChunkManager(mockHttpClient, url);

            //Act
            var function = async () => await chunkManager.Chunk();

            //Assert
            await function.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task Chunk_WhenContentLengthIsZero_ThenChunksCountShouldBeZero()
        {
            //Arrange
            HttpResponseMessage mockResponse = new();
            mockResponse.Content.Headers.ContentLength = 0;
            

            var mockHttpClient = HttpClientHelper
                                .CreateMockHandler(mockResponse, url)
                                .CreateMockHttClient();

            //Act
            var chunkManager = new ChunkManager(mockHttpClient, url);
            var chunks = await chunkManager.Chunk();

            //Assert
            chunks.Count.Should().Be(0);
        }

        [Fact]
        public async Task Chunk_WhenContentLengthIs10MB_ThenChunksLengthShouldBe1MB()
        {
            //Arrange
            HttpResponseMessage mockResponse = new();
            mockResponse.Content.Headers.ContentLength = 10_000_000; //10MB

            var mockHttpClient = HttpClientHelper
                                .CreateMockHandler(mockResponse, url)
                                .CreateMockHttClient();

            //Act
            var chunkManager = new ChunkManager(mockHttpClient, url);
            var chunks = await chunkManager.Chunk();

            //Assert
            chunks.Should().AllSatisfy(x => x.Length.Equals(1_000_000));
        }
        
        [Fact]
        public async Task Chunk_WhenContentLengthIs10MB_ThenSumOfChunksLengthShouldBe10MB()
        {
            //Arrange
            HttpResponseMessage mockResponse = new();
            mockResponse.Content.Headers.ContentLength = 10_000_000; //10MB

            var mockHttpClient = HttpClientHelper
                                .CreateMockHandler(mockResponse, url)
                                .CreateMockHttClient();

            //Act
            var chunkManager = new ChunkManager(mockHttpClient, url);
            var chunks = await chunkManager.Chunk();

            var totalChunksLength = chunks.Select(i => i.Length).Aggregate((x, y) => x + y);

            //Assert
            totalChunksLength.Should().Be(10_000_000);
        }

        [Fact]
        public async Task WhenChunkFile_AllChunksShouldBeUnique()
        {
            //Arrange
            HttpResponseMessage mockResponse = new();
            mockResponse.Content.Headers.ContentLength = 10_000_000; //10MB

            var mockHttpClient = HttpClientHelper
                                .CreateMockHandler(mockResponse, url)
                                .CreateMockHttClient();

            //Act
            var chunkManager = new ChunkManager(mockHttpClient, url);
            var chunks = await chunkManager.Chunk();

            //Assert
            chunks.Should().OnlyHaveUniqueItems();
        }
            [Fact]
        public async Task Chunk_WhenRequestWasNotSuccessfull_ThenThrowError()
        {
            //Arrange
            HttpResponseMessage mockResponse = new();
            mockResponse.StatusCode = HttpStatusCode.BadRequest; 

            var mockHttpClient = HttpClientHelper
                                .CreateMockHandler(mockResponse, url)
                                .CreateMockHttClient();

            //Act
            var chunkManager = new ChunkManager(mockHttpClient, url);          
       
            var function = async () => await chunkManager.Chunk();

            //Assert
            await function.Should().ThrowAsync<ApplicationException>();
        }
    }
}
