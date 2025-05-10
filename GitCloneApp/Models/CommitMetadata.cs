   public class CommitMetadata
    {
        public string Hash { get; set; } = "";
        public string Message { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public List<string> Files { get; set; } = new();
    }