namespace MusicSmash.Database.Interfaces
{
    public interface IRepository<T, J, Y> : IRepository
        where T : Entity<J, Y>
        where J : DBEntity<Y> 
    {
        J Get(Y id);
        void Delete(Y id);
        J Upsert(T entity);
        J[] GetAll();
    }

    public interface IRepository
    {
        IRepository<T, J, Y> Init<T, J, Y>()
                    where T : Entity<J, Y>
                    where J : DBEntity<Y>; 
    }
}