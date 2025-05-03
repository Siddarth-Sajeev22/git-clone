using System;

namespace GitCloneApp.Models
{
    public class Commit
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

        public Commit(string id, string message, DateTime timestamp)
        {
            Id = id;
            Message = message;
            Timestamp = timestamp;
        }
    }
}