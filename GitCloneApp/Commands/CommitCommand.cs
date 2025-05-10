using System.Security.AccessControl;
using System.Text.Json;
using GitCloneApp.Services;

namespace GitCloneApp.Commands
{
    public class CommitCommand: ICommand
    {
        public string _repoPath; 
        public string _gitPath; 
        public CommitCommand(string repoPath, string gitPath)
        {
            _repoPath = repoPath; 
            _gitPath = gitPath;
        }
        public void Execute(string message)
        {
            Console.WriteLine($"Committing changes with message: {message}");
            string indexPath = Path.Combine(_gitPath,"index.json"); 
            if(!File.Exists(indexPath))
            {
                Console.WriteLine("The index file does not exist. Staging area is empty");
                return ; 
            }
            var index = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(indexPath)); 
            if (index == null)
            {
                Console.WriteLine("Nothing to commit "); 
                return ;
            }
            var commitHash = Guid.NewGuid().ToString().Substring(0,8); 
            var commitDir = Path.Combine(_gitPath, "commits", commitHash);
            Directory.CreateDirectory(commitDir);
            foreach (var (file, hash) in index)
            {
                var sourceFile = Path.Combine(_repoPath, file); 
                var destinationFile = Path.Combine(commitDir, file); 
                if(Utils.ComputeHash(sourceFile) != hash)
                {
                    Console.WriteLine("Changes in the source file have not been staged. Please stage them before commiting"); 
                    return ;
                }
                if (sourceFile == null)
                {
                    Console.WriteLine("Source file mentioned cannot be found "); 
                    return ;
                }
                File.Copy(sourceFile, destinationFile, true); 

            }

              // Save commit metadata
            string metadataPath = Path.Combine(_gitPath, "commits.json");
            List<CommitMetadata> commits = File.Exists(metadataPath)
                ? JsonSerializer.Deserialize<List<CommitMetadata>>(File.ReadAllText(metadataPath)) ?? new()
                : new();

            commits.Add(new CommitMetadata
            {
                Hash = commitHash,
                Message = message,
                Timestamp = DateTime.Now,
                Files = index.Keys.ToList()
            });

            File.WriteAllText(metadataPath, JsonSerializer.Serialize(commits, new JsonSerializerOptions { WriteIndented = true }));

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