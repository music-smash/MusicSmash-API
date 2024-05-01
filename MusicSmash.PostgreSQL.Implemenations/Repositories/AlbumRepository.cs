using MusicSmash.Database.Interfaces;
using MusicSmash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusicSmash.Models.Album;

namespace MusicSmash.PostgreSQL.Implemenations.Repositories
{
    internal class AlbumRepository : Repository, IRepository<Album, AlbumDB, long>
    {
        public AlbumRepository(Repository repository) : base(repository)
        {
        }

        public void Delete(long id)
        {
            this.ExecuteQueryWithResults($"DELETE FROM album WHERE id = {id}");
        }

        public AlbumDB Get(long id)
        {
            var result = this.ExecuteQueryWithResults($"SELECT * FROM album WHERE id = {id}");

            if (result.Count() == 0)
                return null;
            if (result.Count() > 1)
                throw new Exception("More than one album with the same id");
            return MapAlbum(result.Single());
        }

        public AlbumDB[] GetAll()
        {
            var result = this.ExecuteQueryWithResults("SELECT * FROM album");
            return result.Select(MapAlbum).ToArray();
        }

        public AlbumDB Upsert(Album entity)
        {
            var result = this.ExecuteQueryWithResults($"SELECT * FROM album WHERE id = {entity.Id}");

            if (result.Count() == 0)
                this.ExecuteQuery($"INSERT INTO album (name, score, release_date, cover) VALUES ('{entity.Name}', {entity.Score}, '{entity.Cover}')");
            else if (result.Count() > 1)
                throw new Exception("More than one album with the same id");
            else 
                this.ExecuteQuery($"UPDATE album SET name = '{entity.Name}', score = {entity.Score}, cover = '{entity.Cover}' WHERE id = {entity.Id}");
            
            var resultAfter = this.ExecuteQueryWithResults($"SELECT * FROM album WHERE name = '{entity.Name}'");
            return MapAlbum(resultAfter.First());
        }


        private static AlbumDB MapAlbum(IDictionary<string, object> entry)
        {
            return new AlbumDB()
            {
                Id = long.Parse(entry["id"].ToString()),
                Name = entry["name"].ToString(),
                Score = int.Parse(entry["score"].ToString()),
                Cover = entry["cover"].ToString()
            };
        }
    }
}
