using MusicSmash.Database.Interfaces;
using MusicSmash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusicSmash.Models.Game;
using static MusicSmash.Models.Round;

namespace MusicSmash.PostgreSQL.Implemenations.Mediators
{
    public class RoundMediator(IConnection connection) : MediatorDBTObject<Round, RoundDB, long>(connection)
    {
        public override Round Mediate(RoundDB entity)
        {
            var gameMediator = new GameMediator(connection);
            var gameRepository = connection.Detach<Game, GameDB, long>();
            long[] gameIds = GetAssociateGames(entity.Id, gameRepository as Repository);
            var games = gameIds.Select(gameRepository.Get);

            return new Round()
            {
                Id = entity.Id,
                Index = entity.Index,
                Games = games.Select(gameMediator.Mediate).ToArray(),
                Owner = new User() { Id = entity.userId }
            };
        }

        private long[] GetAssociateGames(long roundId, Repository repository)
        {
            if (roundId < 0)
                return Array.Empty<long>();
            string query = $"SELECT gameid FROM gameround WHERE roundid = {roundId}";

            var result = repository.ExecuteQueryWithResults(query);
            return result.Select(x => long.Parse(x["gameid"].ToString())).ToArray();
        }
    }
}
