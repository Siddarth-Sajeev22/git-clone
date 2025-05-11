using GitCloneApp.Services;

namespace GitCloneApp.Commands
{
    class BranchCommand: ICommand
    {

        public string _repoPath { get; set; }
        public string _gitPath { get; set; }

        public BranchCommand(GitSettings gitSettings)
        {
            _repoPath = gitSettings.currDir;
            _gitPath = gitSettings.gitPath;
        }

        public void Execute(string branchName)
        {
            var headPath = Path.Combine(_gitPath, "HEAD");
            var refsHeadPath = Path.Combine(_gitPath, "refs", "heads");

            var newBranchRefsHeadsPath = Path.Combine(refsHeadPath, branchName);

            if (File.Exists(newBranchRefsHeadsPath))
            {
                Console.WriteLine($"Branch '{newBranchRefsHeadsPath}' already exists.");
                return;
            }

            var headContent = File.ReadAllText(headPath).Trim();
            var parentBranchPath = headContent.StartsWith("ref: ") ? Path.Combine(_gitPath, headContent.Substring("ref: ".Length)) : null;
            if (parentBranchPath == null)
            {
                throw new Exception("Parent Branch Path returned as null and branch could not be created");               
            }
            parentBranchPath = parentBranchPath.Replace('/', Path.DirectorySeparatorChar);
            var parentBranchHash = File.Exists(parentBranchPath) ? File.ReadAllText(parentBranchPath).Trim(): throw new Exception("Could not get parent branch Hash "); 
            File.WriteAllText(newBranchRefsHeadsPath, parentBranchHash);

        }

        public void Execute(params object[] args)
        {
            if (args.Length > 0 && args[0] is string branchName)
            {
                Execute(branchName); 
            } 
            else 
            {
                throw new ArgumentException("Invalid args passed to branch command"); 
            }
        }

    }
}
