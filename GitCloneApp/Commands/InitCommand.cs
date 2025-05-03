using System;

namespace GitCloneApp.Commands
{
    public class InitCommand
    {
        private readonly string _repoPath;

        public InitCommand(string repoPath)
        {
            _repoPath = repoPath;
        }

        public void Execute()
        {
            var gitDir = Path.Combine(_repoPath, ".mygit");

            if (Directory.Exists(gitDir))
            {
                Console.WriteLine("Repository already initialized.");
                return;
            }

            try
            {
                // Create .mygit directory structure
                Directory.CreateDirectory(gitDir);
                Directory.CreateDirectory(Path.Combine(gitDir, "commits"));
                Directory.CreateDirectory(Path.Combine(gitDir, "branches"));

                // Create main branch with no commits yet
                File.WriteAllText(Path.Combine(gitDir, "branches", "main"), string.Empty);

                // Create HEAD file pointing to main
                File.WriteAllText(Path.Combine(gitDir, "HEAD"), "ref: branches/main");

                Console.WriteLine($"Initialized empty MyGit repository in {_repoPath}\\.mygit");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing repository: {ex.Message}");
            }
        }
    }
}