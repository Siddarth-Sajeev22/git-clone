using GitCloneApp.Commands;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a command.");
            return;
        }

        string command = args[0].ToLower();

        switch (command)
        {
            case "init":
                string path = args.Length > 1 ? args[1] : Directory.GetCurrentDirectory();
                new InitCommand(path).Execute();
                break;

            default:
                Console.WriteLine("Unknown command.");
                break;
        }
    }
}
