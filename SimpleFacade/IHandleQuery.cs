namespace SimpleFacade
{
    public interface IHandleQuery<TQuery, TReturn>
        where TQuery : IQuery<TReturn>
    {
        TReturn Find(TQuery query);
    }
}
