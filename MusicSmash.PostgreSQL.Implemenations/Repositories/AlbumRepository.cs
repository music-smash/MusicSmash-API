using MusicSmash.Database.Interfaces;
using MusicSmash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSmash.PostgreSQL.Implemenations.Repositories
{
    internal class AlbumRepository : Repository, IRepository<Album>
    {
        public AlbumRepository(Repository repository) : base(repository)
        {
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Album Get(int id)
        {
            throw new NotImplementedException();
        }

        public Album[] GetAll()
        {
            throw new NotImplementedException();
        }

        public Album Upsert(Album entity)
        {
            throw new NotImplementedException();
        }
    }
}
