namespace WithSecure.Interview.Services.DownloadManagerService.Chunker
{
    internal class Chunk
    {
        private static int currentOrder = 0;

        public string Id { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int ExecutionOrder { get; set; }
        public byte[] Data { get; set; }
        public int Length => End - Start + 1;

        public Chunk(int start, int end)
        {
            Id = Guid.NewGuid().ToString();
            currentOrder++;
            ExecutionOrder = currentOrder;
            Start = start;
            End = end;
            Data = new byte[Length];
        }

        public static void Flush()
        {
            currentOrder = 0;
        }
    }
}
