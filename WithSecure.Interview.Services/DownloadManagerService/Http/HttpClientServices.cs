namespace WithSecure.Interview.Services.DownloadManagerService.Http
{
    public static class HttpClientServices
    {
        public static async Task<long> GetContentLength(HttpClient client, string url)
        {
            using HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            
            response.EnsureSuccessStatusCode();
            response.EnsureContentIsValid();

            return response.Content.Headers.ContentLength.Value;
        }

        private static void EnsureContentIsValid(this HttpResponseMessage response)
        {
            var contentLength = response.Content.Headers.ContentLength;

            if (contentLength is null)
            {
                throw new ArgumentNullException("It seems there's no file ...");
            }
            else if (contentLength.Value == 0)
            {
                throw new ArgumentOutOfRangeException("File size could not be zero!");
            }
            else if (contentLength.Value > 250_000_000) // 250MB
            {
                throw new ArgumentOutOfRangeException("File size should be less than 200 MB!");
            }
        }
    }
}
