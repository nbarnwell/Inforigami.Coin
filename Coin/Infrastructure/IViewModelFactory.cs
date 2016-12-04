namespace Coin.Infrastructure
{
    public interface IViewModelFactory
    {
        T Create<T>();
    }
}