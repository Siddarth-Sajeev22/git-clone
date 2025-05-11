using System;
using GitCloneApp.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GitCloneApp.Commands
{
    public class CommandResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICommand Resolve(string commandName)
        {
            // Map the command string to the appropriate command type
            return commandName switch
            {
                "init" => _serviceProvider.GetRequiredService<InitCommand>(),
                "add" => _serviceProvider.GetRequiredService<AddCommand>(), 
                "commit" => _serviceProvider.GetRequiredService<CommitCommand>(), 
                "branch" => _serviceProvider.GetRequiredService<BranchCommand>()
            };
        }
    }
}
