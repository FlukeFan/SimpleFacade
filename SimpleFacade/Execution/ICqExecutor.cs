namespace SimpleFacade.Execution
{
    public interface ICqExecutor
    {
        void        Execute(ICommand cmd);
        TReturn     Execute<TReturn>(ICommand<TReturn> cmd);
        TReturn     Execute<TReturn>(IQuery<TReturn> query);
    }
}
