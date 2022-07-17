using WithSecure.Interview.Services.DownloadManagerServiece.Helper;

namespace WithSecure.Interview.Services.DownloadManagerServiece.Chunker
{
    internal class ChunkManager
    {
        private readonly int _chunkCount;
        private int _chuckSize = 0;

        public ChunkManager(int chunkCount = 10)
        {
            _chunkCount = chunkCount;
        }

        public List<Chunk> Chunk(long contentLength)
        {
            try
            {
                var chunks = new List<Chunk>();
                CalculateChunkSize(contentLength);
                for (int i = 0; i < contentLength; i = i + _chuckSize)
                {
                    chunks.Add(new Chunk(i, i + _chuckSize - 1));
                }
                Flush();
                return chunks;
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Something is wrong with Chunk method.", exception);
            }
        }

        public void Flush()
        {
            Chunker.Chunk.Flush();
        }

        public byte[] MergeChunks(IEnumerable<Chunk> chunks)
        {
            var file = new List<byte>();
            foreach (var chunk in chunks.OrderBy(x => x.ExecutionOrder))
            {
                file.AddRange(chunk.Data);
            }
            return file.ToArray();
        }
        private void CalculateChunkSize(long contentLength)
        {
            _chuckSize = (int)Math.Floor((double)contentLength / _chunkCount);
        }
    }
}
