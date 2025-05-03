using System;

namespace GitCloneApp.Commands
{
    public class CommitCommand
    {
        public void Execute(string message)
        {
            Console.WriteLine($"Committing changes with message: {message}");
            // Add logic to commit changes
        }
    }
}