using WithSecure.Interview.Services.DownloadManagerServiece.Helper;

namespace WithSecure.Interview.Services.DownloadManagerServiece.Chunker
{
    internal class ChunkManager
    {
        private readonly HttpClient _client;
        private readonly int _chunkCount;
        private readonly string url;
        private int _chuckSize;
        public ChunkManager(string sourceUrl, int chunkCount = 10)
        {
            url = sourceUrl;
            _chunkCount = chunkCount;
            _client = new HttpClientFactory().CreateClient();
        }
        
        public ChunkManager(HttpClient client , string sourceUrl, int chunkCount = 10)
        {
            url = sourceUrl;
            _chunkCount = chunkCount;
            _client = client;
        }
        public async Task<List<Chunk>> Chunk()
        {
            try
            {
                var chunks = new List<Chunk>();
                    using (HttpResponseMessage response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();
                        var contentLength = response.Content.Headers.ContentLength;
                        ArgumentNullException.ThrowIfNull(contentLength);
                        CalculateChunkSize(contentLength.Value);
                        for (int i = 0; i < contentLength; i = i + _chuckSize)
                        {
                            chunks.Add(new Chunk(i, i + _chuckSize - 1));
                        }
                        return chunks;
                    }
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Something is wrong with Chunk method.", exception);
            }
        }

        public byte[] MergeChunks(IEnumerable<Chunk> chunks)
        {
            var file = new List<byte>();
            foreach (var chunk in chunks.OrderBy(x => x.ExecutionOrder))
            {
                file.AddRange(chunk.Data);
            }
            Chunker.Chunk.Flush();
            return file.ToArray();
        }
        private void CalculateChunkSize(long contentLength)
        {
            _chuckSize = (int)Math.Floor((double)contentLength / _chunkCount);
        }
    }
}
