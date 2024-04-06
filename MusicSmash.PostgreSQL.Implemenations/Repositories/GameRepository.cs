using MusicSmash.Database.Interfaces;
using MusicSmash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSmash.PostgreSQL.Implemenations.Repositories
{
    internal class GameRepository : Repository, IRepository<Game.GameDB>
    {
        public GameRepository(Repository repository) : base(repository)
        {
        }

        public void Delete(string id)
        {
            this.ExecuteQuery($"DELETE FROM games WHERE id = {id}");
        }

        public Game.GameDB Get(string id)
        {
            var result = this.ExecuteQuery($"SELECT * FROM games WHERE id = {id}");
            if (result.Count() == 0)
                return null;
            if (result.Count() > 1)
                throw new Exception("More than one game with the same id");
            return MapGame(result.Single());
        }

        public Game.GameDB[] GetAll()
        {
            var result = this.ExecuteQuery("SELECT * FROM games");
            return result.Select(MapGame).ToArray();
        }

        public Game.GameDB Upsert(Game.GameDB entity)
        {
            var result = this.ExecuteQuery($"SELECT * FROM games WHERE id = {entity.id}");

            if (result.Count() == 0)
                this.ExecuteQuery($"INSERT INTO games (id, left, right, winner) VALUES ({entity.id}, {entity.Left}, {entity.Right}, {entity.Winner})");
            else if (result.Count() > 1)
                throw new Exception("More than one game with the same id");
            else
                this.ExecuteQuery($"UPDATE games SET left = {entity.Left}, right = {entity.Right}, winner = {entity.Winner} WHERE id = {entity.id}");

            var resultAfter = this.ExecuteQuery($"SELECT * FROM games WHERE id = {entity.id}");
            return MapGame(resultAfter.Single());
        }

        private Game.GameDB MapGame(IDictionary<string, object> dictionary)
        {
            return new Game.GameDB()
            {
                id = dictionary["id"]?.ToString(),
                Left = dictionary["left"]?.ToString(),
                Right = dictionary["right"]?.ToString(),
                Winner = dictionary["winner"]?.ToString()
            };
        }
    }
}
