using Moq;
using Moq.Protected;

namespace WithSecure.Interview.Services.Tests.DownloadManagerService.Helper
{
    internal static class HttpClientHelper
    {
        internal static Mock<HttpMessageHandler> CreateMockHandler(HttpResponseMessage mockResponse)
        {
            var mockHtppHandler = new Mock<HttpMessageHandler>();

            mockHtppHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            return mockHtppHandler;
        }

        internal static HttpClient CreateMockHttClient(this Mock<HttpMessageHandler> handler)
        {
            return new HttpClient(handler.Object);
        }
    }
}
