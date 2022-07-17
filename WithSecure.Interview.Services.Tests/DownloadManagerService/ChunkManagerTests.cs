using WithSecure.Interview.Services.DownloadManagerServiece.Chunker;
using FluentAssertions;
using WithSecure.Interview.Services.Tests.DownloadManagerService.Helper;
using System.Net;

namespace WithSecure.Interview.Services.Tests.DownloadManagerService
{
    public class ChunkManagerTests
    {
        [Fact]
        public void Chunk_WhenContentLengthIs10MB_ThenChunksLengthShouldBe1MB()
        {
            //Arrange
            var contentLength = 10_000_000;

            //Act
            var chunkManager = new ChunkManager();
            var chunks = chunkManager.Chunk(contentLength);

            //Assert
            chunks.Should().AllSatisfy(x => x.Length.Equals(1_000_000));
        }
        
        [Fact]
        public void Chunk_WhenContentLengthIs10MB_ThenSumOfChunksLengthShouldBe10MB()
        {
            //Arrange
            var contentLength = 10_000_000;

            //Act
            var chunkManager = new ChunkManager();
            var chunks = chunkManager.Chunk(contentLength);

            var totalChunksLength = chunks.Select(i => i.Length).Aggregate((x, y) => x + y);

            //Assert
            totalChunksLength.Should().Be(10_000_000);
        }

        [Fact]
        public void WhenChunkFile_AllChunksShouldBeUnique()
        {
            //Arrange
            var contentLength = 10_000_000;

            //Act
            var chunkManager = new ChunkManager();
            var chunks = chunkManager.Chunk(contentLength);

            //Assert
            chunks.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void WhenMergeChunks_ThenSizeOfMergedChunksShouldBeEqualToContentLength()
        {
            //Arrange
            var contentLength = 10_000_000;

            //Act
            var chunkManager = new ChunkManager();
            var chunks = chunkManager.Chunk(contentLength);
            var mergedChunks = chunkManager.MergeChunks(chunks);
            //Assert
            mergedChunks.Length.Should().Be(contentLength);
        }

        [Fact]
        public void WhenChunkOperationIsDone_ChunkShouldBeFlushed()
        {
            //Arrange
            var contentLength = 10_000_000;

            //Act
            var chunkManager = new ChunkManager();
            var chunks1_ExecutionOrderOfFirstNode = chunkManager.Chunk(contentLength).First().ExecutionOrder;
            var chunks2_ExecutionOrderOfFirstNode = chunkManager.Chunk(contentLength).First().ExecutionOrder;

            //Assert
            chunks1_ExecutionOrderOfFirstNode.Should().Be(1).Equals(chunks2_ExecutionOrderOfFirstNode);
        }
    }
}
