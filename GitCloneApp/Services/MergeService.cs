using GitCloneApp.Models;
using System;

namespace GitCloneApp.Services
{
    public class MergeService
    {
        public void MergeBranch(Repository repository, string branchName)
        {
            Console.WriteLine($"Merging branch {branchName} into repository at {repository.Path}...");
            // Add logic to merge branches
        }
    }
}