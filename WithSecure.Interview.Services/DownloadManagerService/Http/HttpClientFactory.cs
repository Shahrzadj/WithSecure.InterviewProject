namespace WithSecure.Interview.Services.DownloadManagerService.Http
{
    internal class HttpClientFactory
    {
        public HttpClient CreateClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "downloader manager");
            return httpClient;
        }
    }
}
