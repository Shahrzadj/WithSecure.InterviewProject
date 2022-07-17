namespace WithSecure.Interview.Services.DownloadManagerService.Http
{
    internal interface IHttpClientServices
    {
        Task<long> GetContentLength(HttpClient client, string url);
    }
}