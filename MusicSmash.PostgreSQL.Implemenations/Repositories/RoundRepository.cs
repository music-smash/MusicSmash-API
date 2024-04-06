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
    internal class RoundRepository : Repository, IRepository<Round.RoundDB>
    {
        public RoundRepository(Repository repository) : base(repository)
        {
        }

        public void Delete(string id)
        {
            this.ExecuteQuery($"DELETE FROM rounds WHERE id = {id}");
        }

        public Round.RoundDB Get(string id)
        {
            var result = this.ExecuteQuery($"SELECT * FROM rounds WHERE id = {id}");
            if (result.Count() == 0)
                return null;
            if (result.Count() > 1)
                throw new Exception("More than one round with the same id");
            return MapRound(result.Single());
        }

        public Round.RoundDB[] GetAll()
        {
            var result = this.ExecuteQuery("SELECT * FROM rounds");
            return result.Select(MapRound).ToArray();
        }

        public Round.RoundDB Upsert(Round.RoundDB entity)
        {
            var result = this.ExecuteQuery($"SELECT * FROM rounds WHERE id = {entity.Index}");

            if (result.Count() == 0)
                this.ExecuteQuery($"INSERT INTO rounds (id, index, userid) VALUES ({entity.id}, {entity.Index}, {entity.userId})");
            else if (result.Count() > 1)
                throw new Exception("More than one round with the same id");
            else
                this.ExecuteQuery($"UPDATE rounds SET index = {entity.Index}, userid = {entity.userId} WHERE id = {entity.id}");

            var resultAfter = this.ExecuteQuery($"SELECT * FROM rounds WHERE id = {entity.id}");
            return MapRound(resultAfter.Single());
        }


        private Round.RoundDB MapRound(IDictionary<string, object> dictionary)
        {
            return new Round.RoundDB()
            {
                id = dictionary["id"].ToString(),
                Index = (int)dictionary["index"],
                userId = dictionary["userid"].ToString(),
            };
        }
    }
}
