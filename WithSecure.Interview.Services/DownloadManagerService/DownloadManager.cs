using WithSecure.Interview.Services.DownloadManagerServiece.Chunker;
using WithSecure.Interview.Services.DownloadManagerServiece.Helper;
using System.Net.Http.Headers;

namespace WithSecure.Interview.Services.DownloadManagerServiece
{
    public class DownloadManager
    {
        private readonly string _url;
        private readonly string _fileExtension;

        public DownloadManager(string url)
        {
            _url = url;
            _fileExtension = Path.GetExtension(url);
        }

        public async Task<byte[]> GetByteArrayAsync()
        {
            var chunkManager = new ChunkManager(_url);
            var chunks = await chunkManager.Chunk().ConfigureAwait(false);
            var tasks = new List<Task>();
            foreach (var chunk in chunks)
            {
                tasks.Add(Task.Run(() =>
                {
                    var chunkBytes = GetFileAsync(_url, chunk).Result;
                    chunk.Data = chunkBytes;
                }));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            var finalByteArray = chunkManager.MergeChunks(chunks);
            ArgumentNullException.ThrowIfNull(finalByteArray);
            return finalByteArray;
        }
        private async Task<byte[]> GetFileAsync(string filePath, Chunk chunk)
        {
            using (var memory = new MemoryStream())
            {
                byte[] totalBuffer = new byte[chunk.Length];
                int receivedBytes = chunk.Start;

                using (HttpClient client = new HttpClientFactory().CreateClient())
                {
                    client.DefaultRequestHeaders.Range = new RangeHeaderValue(chunk.Start, chunk.End);
                    using (Stream stream = await client.GetStreamAsync(filePath).ConfigureAwait(false))
                    {
                        await stream.CopyToAsync(memory).ConfigureAwait(false);
                        await stream.ReadAsync(totalBuffer, 0, chunk.Length).ConfigureAwait(false);
                    }
                    return memory.ToArray();
                }
            }
        }


        // I used this method to check if the downloaded file is correct or not!
        public async Task DownloadFileAsync(string outputUrl = "c:/temp")
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
