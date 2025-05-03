using System;

namespace GitCloneApp.Commands
{
    public class MergeCommand
    {
        public void Execute(string branchName)
        {
            Console.WriteLine($"Merging branch: {branchName}");
            // Add logic to merge branches
        }
    }
}