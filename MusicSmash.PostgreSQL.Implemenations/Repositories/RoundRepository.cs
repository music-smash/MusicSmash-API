using MusicSmash.Database.Interfaces;
using MusicSmash.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusicSmash.Models.Round;

namespace MusicSmash.PostgreSQL.Implemenations.Repositories
{
    internal class RoundRepository : Repository, IRepository<Round, RoundDB, long>
    {
        public RoundRepository(Repository repository) : base(repository)
        {
        }

        public void Delete(long id)
        {
            this.ExecuteQueryWithResults($"DELETE FROM round WHERE id = {id}");
        }

        public RoundDB Get(long id)
        {
            var result = this.ExecuteQueryWithResults($"SELECT * FROM round WHERE id = {id}");
            if (result.Count() == 0)
                return null;
            if (result.Count() > 1)
                throw new Exception("More than one round with the same id");
            return MapRound(result.Single());
        }

        public RoundDB[] GetAll()
        {
            var result = this.ExecuteQueryWithResults("SELECT * FROM round");
            return result.Select(MapRound).ToArray();
        }

        public RoundDB Upsert(Round entity)
        {
            var result = this.ExecuteQueryWithResults($"SELECT * FROM round WHERE id = {entity.Index}");

            if (result.Count() == 0)
                this.ExecuteQuery($"INSERT INTO round (index, userid) VALUES ({entity.Index}, '{entity.Owner.Id}')");
            else if (result.Count() > 1)
                throw new Exception("More than one round with the same id");
            else
                this.ExecuteQuery($"UPDATE round SET index = {entity.Index}, userid = '{entity.Owner.Id}' WHERE id = {entity.Id}");

            var resultAfter = this.ExecuteQueryWithResults($"SELECT * FROM round WHERE index = {entity.Index} AND userid = '{entity.Owner.Id}' ORDER BY id DESC").First();

            var round = MapRound(resultAfter);

            foreach (var game in entity.Games)
            {
                this.ExecuteQuery($"INSERT INTO gameround (gameid, roundid) VALUES ({game.Id}, {round.Id})");
            }

            return round;
        }


        private Round.RoundDB MapRound(IDictionary<string, object> dictionary)
        {
            return new Round.RoundDB()
            {
                Id = long.Parse(dictionary["id"].ToString()),
                Index = int.Parse(dictionary["index"].ToString()),
                userId = dictionary["userid"].ToString(),
            };
        }
    }
}
