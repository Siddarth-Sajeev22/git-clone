namespace GitCloneApp.Models
{
    public class Repository
    {
        public string Path { get; set; }

        public Repository(string path)
        {
            Path = path;
        }
    }
}