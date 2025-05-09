using System;
using GitCloneApp.Services;

namespace GitCloneApp.Commands
{
    public class InitCommand: ICommand
    {
        private readonly GitSettings _gitSettings;

        public InitCommand(GitSettings gitSettings)
        {
            _gitSettings = gitSettings;
        }

        public void Execute()
        {
            var gitDir = _gitSettings.gitPath;

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

                Console.WriteLine($"Initialized empty MyGit repository in {_gitSettings.gitPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing repository: {ex.Message}");
            }
        }

        public void Execute(params object[] args)
        {
            Execute();
        }
    }
}