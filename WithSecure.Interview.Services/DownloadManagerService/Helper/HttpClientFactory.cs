namespace WithSecure.Interview.Services.DownloadManagerServiece.Helper
{
    internal class HttpClientFactory
    {
        public HttpClient CreateClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "downloader library");
            return httpClient;
        }
    }
}
