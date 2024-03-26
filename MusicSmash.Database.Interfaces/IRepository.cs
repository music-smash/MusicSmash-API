namespace MusicSmash.Database.Interfaces
{
    public interface IRepository<T> : IRepository
    {
        T Get(int id);
        void Delete(int id);
        T Upsert(T entity);
        T[] GetAll();
    }

    public interface IRepository
    {
        IRepository<T> Init<T>();
    }
}