using MusicSmash.Database.Interfaces;
using MusicSmash.Models;
using static MusicSmash.Models.Game;

namespace MusicSmash.PostgreSQL.Implemenations.Repositories
{
    internal class GameRepository : Repository, IRepository<Game, GameDB, long>
    {
        public GameRepository(Repository repository) : base(repository)
        {
        }

        public void Delete(long id)
        {
            this.ExecuteQueryWithResults($"DELETE FROM game WHERE id = {id}");
        }

        public GameDB Get(long id)
        {
            var result = this.ExecuteQueryWithResults($"SELECT * FROM game WHERE id = {id}");
            if (result.Count() == 0)
                return null;
            if (result.Count() > 1)
                throw new Exception("More than one game with the same id");
            return MapGame(result.Single());
        }

        public GameDB[] GetAll()
        {
            var result = this.ExecuteQueryWithResults("SELECT * FROM game");
            return result.Select(MapGame).ToArray();
        }

        public GameDB Upsert(Game entity)
        {
            var result = this.ExecuteQueryWithResults($"SELECT * FROM game WHERE id = {entity.Id}").ToList();

            var equal = "="; var isNull = "IS";

            if (result.Count() == 0)
                this.ExecuteQuery($"INSERT INTO game (leftalbumid, rightalbumid, winneralbumid) VALUES ({entity.Left.GetId()}, {entity.Right.GetId()}, {entity.Winner?.GetId()})");
            else if (result.Count() > 1)
                throw new Exception("More than one game with the same id");
            else
                this.ExecuteQuery($"UPDATE game SET leftalbumid = {entity.Left.GetId()}, rightalbumid = {entity.Right.GetId()}, winneralbumid = {entity.Winner?.GetId()} WHERE id = {entity.Id}");

            var resultAfter = this.ExecuteQueryWithResults($"SELECT * FROM game WHERE leftalbumid {(entity.Left.Id > 0 ? equal : isNull)} {entity.Left.GetId()} AND rightalbumid {(entity.Right.Id > 0 ? equal : isNull)} {entity.Right.GetId()} AND winneralbumid {((entity.Winner?.Id ?? -1) > 0 ? equal : isNull)} {entity.Winner?.GetId()} ORDER BY id DESC").ToList();
            return MapGame(resultAfter.First());
        }

        private GameDB MapGame(IDictionary<string, object> dictionary)
        {
            foreach (var key in dictionary.Keys) //normalizing null opr empty ids
            {
                dictionary[key] = string.IsNullOrWhiteSpace(dictionary[key]?.ToString()) ? "-1" : dictionary[key];
            }
            return new GameDB()
            {
                Id = long.Parse(dictionary["id"].ToString()),
                Left = long.Parse(dictionary["leftalbumid"]?.ToString()),
                Right = long.Parse(dictionary["rightalbumid"]?.ToString()),
                Winner = long.Parse(dictionary["winneralbumid"]?.ToString())
            };
        }
    }
}
