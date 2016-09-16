namespace SimpleFacade
{
    public interface ICommand {}

    public interface ICommand<in TReturn> { }
}
