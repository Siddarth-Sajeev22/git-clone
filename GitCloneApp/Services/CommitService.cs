using GitCloneApp.Models;
using System;

namespace GitCloneApp.Services
{
    public class CommitService
    {
        public void CreateCommit(Repository repository, string message)
        {
            Console.WriteLine($"Creating commit in repository at {repository.Path} with message: {message}");
            // Add logic to create a commit
        }
    }
}