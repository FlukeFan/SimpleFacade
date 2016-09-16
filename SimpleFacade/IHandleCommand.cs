namespace SimpleFacade
{
    public interface IHandleVoidCommand<TCmd>
        where TCmd : ICommand
    {
        void Execute(TCmd cmd);
    }

    public interface IHandleCommand<TCmd, TReturn>
        where TCmd : ICommand<TReturn>
    {
        TReturn Execute(TCmd cmd);
    }
}
