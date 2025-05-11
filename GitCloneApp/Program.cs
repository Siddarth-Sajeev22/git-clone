using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GitCloneApp.Services;
using GitCloneApp.Commands;

class Program
{
    static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Register services for dependency injection
                services.AddSingleton<GitSettings>();
                services.AddTransient<InitCommand>();
                services.AddTransient<AddCommand>();
                services.AddTransient<BranchCommand>();
                services.AddTransient<CommitCommand>();
                services.AddSingleton<CommandResolver>();  // Register the CommandResolver
            })
            .Build();
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a command.");
            return;
        }

        string command = args[0].ToLower();
        var commandResolver = host.Services.GetRequiredService<CommandResolver>();  // Resolving the resolver
        var gitCommand = commandResolver.Resolve(command);  // Resolving the command based on user input
        if (gitCommand == null)
        {
            Console.WriteLine($"Unknown command: {command}");
            return;
        }
        else {
            try {
                gitCommand.Execute(args.Skip(1).ToArray());
            }
            catch (Exception ex) {
                Console.WriteLine($"Error executing command: {ex.Message}");
            }
        }
    }
}
