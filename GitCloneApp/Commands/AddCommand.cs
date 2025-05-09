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
                var hash = ComputeHash(fileContent); 
                Dictionary<string, string> index = File.Exists(indexPath) ? JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(indexPath)) ?? new Dictionary<string, string>() : new Dictionary<string, string>();

                index[filePath] = hash ;
                File.WriteAllText(indexPath, JsonSerializer.Serialize(index, new JsonSerializerOptions{ WriteIndented = true}));
            }
        }

        private string ComputeHash(string fileContent)
        {
            using var sha = SHA256.Create();
            byte[] hashBytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(fileContent));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }


        public void Execute(params object[] args)
        {
            if (args.Length > 0 && args[0] is string[] files)
            {
                Execute(files); 
            } 
            else 
            {
                throw new ArgumentException("Invalid args passed to add command"); 
            }
        }
    }
}