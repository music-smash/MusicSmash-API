
namespace MusicSmash.Database.Interfaces
{
    public class Entity<DBType, DBIdType> where DBType : DBEntity<DBIdType>
    {
        public required DBIdType Id;

        public virtual string GetId()
        {
            return Id?.ToString() ?? "null";
        }
    }

    public class DBEntity<TId>
    {
        public TId Id { get; set; }

        public virtual string GetId()
        {
            return Id?.ToString() ?? "null";
        }
    }
}
