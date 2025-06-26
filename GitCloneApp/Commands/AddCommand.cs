using System.Security.Cryptography;
using System.Text.Json;
using GitCloneApp.Services;

namespace GitCloneApp.Commands
{
    public class AddCommand: ICommand
    {
        public GitSettings _gitSettings { get; set; }
        public AddCommand(GitSettings gitSettings)
        {
            _gitSettings = gitSettings;       
        }
        public void Execute(string[] files)
        {
            foreach(var file in files){
                var indexPath = Path.Combine(_gitSettings.gitPath, "index.json");
                var filePath = Path.Combine(_gitSettings.currDir, file);
                var fileContent = File.ReadAllText(filePath);
                var hash = Utils.ComputeHash(fileContent); 
                Dictionary<string, string> index = File.Exists(indexPath) ? JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(indexPath)) ?? new Dictionary<string, string>() : new Dictionary<string, string>();

                index[file] = hash ;
                File.WriteAllText(indexPath, JsonSerializer.Serialize(index, new JsonSerializerOptions{ WriteIndented = true}));
            }
        }

        public void Execute(params object[] args)
        {
            if (args.Length > 0)
            {
                var files = args.Select(a => a?.ToString() ?? string.Empty).ToArray();
                Execute(files);
            }
            else
            {
                throw new ArgumentException("Invalid args passed to add command"); 
            }
        }
    }
}