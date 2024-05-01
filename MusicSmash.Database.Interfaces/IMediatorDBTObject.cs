using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSmash.Database.Interfaces
{
    public abstract class MediatorDBTObject<T, J, Y>(IConnection connection)
                where T : Entity<J, Y>
                where J : DBEntity<Y>
    {
        private readonly IConnection _connection = connection;

        public abstract T Mediate(J entity);
    }
}
