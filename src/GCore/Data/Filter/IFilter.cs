namespace GCore.Data.Filter;

public interface IFilter<in T>
{
    bool Passes(T elem);
}