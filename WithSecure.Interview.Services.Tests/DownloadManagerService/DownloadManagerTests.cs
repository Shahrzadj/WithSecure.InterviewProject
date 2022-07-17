using FluentAssertions;
using WithSecure.Interview.Services.DownloadManagerServiece;

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
            await action.Should().ThrowAsync<ApplicationException>();
        }
    }
}