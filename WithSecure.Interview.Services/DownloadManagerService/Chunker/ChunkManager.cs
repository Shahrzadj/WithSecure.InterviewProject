using WithSecure.Interview.Services.DownloadManagerServiece.Helper;

namespace WithSecure.Interview.Services.DownloadManagerServiece.Chunker
{
    internal class ChunkManager
    {
        private readonly int ChunkCount;
        private readonly string Url;
        private int ChuckSize;
        public ChunkManager(string sourceUrl, int chunkCount = 10)
        {
            Url = sourceUrl;
            ChunkCount = chunkCount;
        }
        public async Task<List<Chunk>> Chunk()
        {
            try
            {
                var chunks = new List<Chunk>();
                using (HttpClient client = new HttpClientFactory().CreateClient())
                {
                    using (HttpResponseMessage response = await client.GetAsync(Url, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();
                        var contentLength = response.Content.Headers.ContentLength;
                        ArgumentNullException.ThrowIfNull(contentLength);
                        CalculateChunkSize(contentLength.Value);
                        for (int i = 0; i < contentLength; i = i + ChuckSize)
                        {
                            chunks.Add(new Chunk(i, i + ChuckSize - 1));
                        }
                        return chunks;
                    }
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
            ChuckSize = (int)Math.Floor((double)contentLength / ChunkCount);
        }
    }
}
