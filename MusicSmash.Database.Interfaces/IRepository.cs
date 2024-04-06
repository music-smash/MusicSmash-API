namespace MusicSmash.Database.Interfaces
{
    public interface IRepository<T> : IRepository
    {
        T Get(string id);
        void Delete(string id);
        T Upsert(T entity);
        T[] GetAll();
    }

    public interface IRepository
    {
        IRepository<T> Init<T>();
    }
}