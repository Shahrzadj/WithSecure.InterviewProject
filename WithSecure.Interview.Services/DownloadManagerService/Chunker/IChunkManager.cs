namespace WithSecure.Interview.Services.DownloadManagerService.Chunker
{
    internal interface IChunkManager
    {
        List<Chunk> Chunk(long contentLength);
        byte[] MergeChunks(IEnumerable<Chunk> chunks);
        void Flush();
    }
}