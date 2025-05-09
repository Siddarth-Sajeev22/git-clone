using System.Collections.Concurrent;
using System.IO;

namespace GitCloneApp.Services
{
    public class GitSettings
    {
        public string gitPath { get; set; }
        public string currDir { get; set; }

        public GitSettings()
        {
            // Get the current repository path and append .mygit
            gitPath = Path.Combine(Directory.GetCurrentDirectory(), ".mygit");
            currDir = Directory.GetCurrentDirectory();
            
        }
    }
}