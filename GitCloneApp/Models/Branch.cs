namespace GitCloneApp.Models
{
    public class Branch
    {
        public string Name { get; set; }
        public string CommitId { get; set; }

        public Branch(string name, string commitId)
        {
            Name = name;
            CommitId = commitId;
        }
    }
}