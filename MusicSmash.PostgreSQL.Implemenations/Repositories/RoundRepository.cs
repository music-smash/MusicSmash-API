using MusicSmash.Database.Interfaces;
using MusicSmash.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSmash.PostgreSQL.Implemenations.Repositories
{
    internal class RoundRepository : Repository, IRepository<Round>
    {
        public RoundRepository(Repository repository) : base(repository)
        {
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Round Get(int id)
        {
            throw new NotImplementedException();
        }

        public Round[] GetAll()
        {
            throw new NotImplementedException();
        }

        public Round Upsert(Round entity)
        {
            throw new NotImplementedException();
        }

    }
}
