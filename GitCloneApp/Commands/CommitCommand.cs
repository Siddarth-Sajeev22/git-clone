using System.Text.Json;
using GitCloneApp.Services;

namespace GitCloneApp.Commands
{
    public class CommitCommand : ICommand
    {
        public string _repoPath { get; set; }
        public string _gitPath { get; set; }
        public CommitCommand(GitSettings gitSettings)
        {
            _repoPath = gitSettings.currDir;
            _gitPath = gitSettings.gitPath;
        }
        public void Execute(string message)
        {
            Console.WriteLine($"Committing changes with message: {message}");
            string indexPath = Path.Combine(_gitPath, "index.json");
            if (!File.Exists(indexPath))
            {
                Console.WriteLine("The index file does not exist. Staging area is empty");
                return;
            }
            var index = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(indexPath));
            if (index == null)
            {
                Console.WriteLine("Nothing to commit ");
                return;
            }
            string headPath = Path.Combine(_gitPath, "HEAD");
            if (!File.Exists(headPath))
            {
                Console.WriteLine("HEAD not found. Repository might not be initialized.");
                return;
            }

            string headContent = File.ReadAllText(headPath).Trim();
            if (!headContent.StartsWith("ref: "))
            {
                Console.WriteLine("Invalid HEAD format.");
                return;
            }

            string branchRefPath = headContent.Substring("ref: ".Length); // "refs/heads/main"
            string branchFullPath = Path.Combine(_gitPath, branchRefPath);

            string parentHash = File.Exists(branchFullPath)
                ? File.ReadAllText(branchFullPath).Trim()
                : string.Empty;

            var commitHash = Guid.NewGuid().ToString().Substring(0, 8);
            var commitDir = Path.Combine(_gitPath, "commits", commitHash);
            Directory.CreateDirectory(commitDir);
            foreach (var (file, hash) in index)
            {
                var sourceFile = Path.Combine(_repoPath, file);
                var destinationFile = Path.Combine(commitDir, file);
                var recentHash = Utils.ComputeHash(File.ReadAllText(sourceFile));
                if ( recentHash != hash)
                {
                    Console.WriteLine("Changes in the source file have not been staged. Please stage them before commiting");
                    return;
                }
                if (sourceFile == null)
                {
                    Console.WriteLine("Source file mentioned cannot be found ");
                    return;
                }
                Directory.CreateDirectory(Path.GetDirectoryName(destinationFile)!);
                File.Copy(sourceFile, destinationFile, true);

            }

            // Save commit metadata
            var commitMeta = new CommitMetadata
            {
                Hash = commitHash,
                Message = message,
                Timestamp = DateTime.UtcNow,
                Files = index.Keys.ToList(),
                ParentHash = parentHash
            };

            File.WriteAllText(
                Path.Combine(commitDir, $"{commitHash}.json"),
                JsonSerializer.Serialize(commitMeta, new JsonSerializerOptions { WriteIndented = true })
            );

                // Update branch pointer
            Directory.CreateDirectory(Path.GetDirectoryName(branchFullPath)!); // Ensure "refs/heads"
            File.WriteAllText(branchFullPath, commitHash);


            File.WriteAllText(indexPath, "{}");
            Console.WriteLine($"Commit created with commit id {commitHash}");
        }

        public void Execute(params object[] args)
        {
            if (args.Length > 0 && args[0] is string commitMessage)
            {
                Execute(commitMessage);
            }
            else
            {
                throw new ArgumentException("Invalid args passed to commit command");
            }
        }
    }
}