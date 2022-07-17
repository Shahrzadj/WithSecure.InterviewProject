namespace WithSecure.Interview.Services.DownloadManagerService.Http
{
    internal class HttpClientFactory
    {
        public HttpClient CreateClient()
        {
            var httpClient = new HttpClient();
            if (httpClient.DefaultRequestHeaders.Contains("User-Agent"))
            {
                httpClient.DefaultRequestHeaders.Remove("User-Agent");
            }
            httpClient.DefaultRequestHeaders.Add("User-Agent", "downloader manager");
            return httpClient;
        }
    }
}
