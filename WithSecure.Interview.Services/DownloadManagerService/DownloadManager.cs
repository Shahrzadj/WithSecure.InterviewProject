using WithSecure.Interview.Services.DownloadManagerServiece.Chunker;
using WithSecure.Interview.Services.DownloadManagerServiece.Helper;
using System.Net.Http.Headers;
using WithSecure.Interview.Services.DownloadManagerService.Helper;

namespace WithSecure.Interview.Services.DownloadManagerServiece
{
    public class DownloadManager
    {
        private readonly string _url;
        private readonly string _fileExtension;
        private readonly HttpClient _client;

        public DownloadManager(string url)
        {
            _url = url;
            _fileExtension = Path.GetExtension(url);
            _client = new HttpClientFactory().CreateClient();
        }

        public DownloadManager(HttpClient client, string url)
        {
            _client = client;
            _url = url;
            _fileExtension = Path.GetExtension(url);
        }

        public async Task<byte[]> GetByteArrayAsync()
        {
            try
            {
                var contentLength = await HttpClientServices.GetContentLength(_client, _url).ConfigureAwait(false);

                var chunkManager = new ChunkManager();
                var chunks = chunkManager.Chunk(contentLength);
                var tasks = new List<Task>();
                foreach (var chunk in chunks)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        var chunkBytes = DownloadChunkAsync(_url, chunk).Result;
                        Console.WriteLine($"... chunk #{chunk.ExecutionOrder} downloaded successfully! ...");
                        chunk.Data = chunkBytes;
                    }));
                }

                await Task.WhenAll(tasks).ConfigureAwait(false);

                var finalByteArray = chunkManager.MergeChunks(chunks);
                ArgumentNullException.ThrowIfNull(finalByteArray);

                return finalByteArray;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task<byte[]> DownloadChunkAsync(string filePath, Chunk chunk)
        {
            using (var memory = new MemoryStream())
            {
                byte[] totalBuffer = new byte[chunk.Length];
                int receivedBytes = chunk.Start;

                _client.DefaultRequestHeaders.Range = new RangeHeaderValue(chunk.Start, chunk.End);
                using (Stream stream = await _client.GetStreamAsync(filePath).ConfigureAwait(false))
                {
                    await stream.CopyToAsync(memory).ConfigureAwait(false);
                    await stream.ReadAsync(totalBuffer, 0, chunk.Length).ConfigureAwait(false);
                }
                return memory.ToArray();

            }
        }


        // I used this method to check if the downloaded file is correct or not!
        public async Task DownloadFileAsync(string outputUrl = "c:/")
        {
            if (!Directory.Exists(outputUrl))
            {
                throw new Exception($"Directory not exist: {outputUrl}");
            }

            var file = await GetByteArrayAsync().ConfigureAwait(false);

            SaveByteArrayToFile(file, $"{outputUrl}/file_{Guid.NewGuid()}.{_fileExtension}");
        }

        private void SaveByteArrayToFile(byte[] data, string filePath)
        {
            File.WriteAllBytes(filePath, data);
        }
    }
}
