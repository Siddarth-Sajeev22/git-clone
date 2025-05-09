namespace GitCloneApp.Commands{
    public interface ICommand{
        void Execute(params object[] args);
    }
}
