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

        public void Delete(string id)
        {
            this.ExecuteQuery($"DELETE FROM albums WHERE id = {id}");
        }

        public Album Get(string id)
        {
            var result = this.ExecuteQuery($"SELECT * FROM albums WHERE id = {id}");

            if (result.Count() == 0)
                return null;
            if (result.Count() > 1)
                throw new Exception("More than one album with the same id");
            return MapAlbum(result.Single());
        }

        public Album[] GetAll()
        {
            var result = this.ExecuteQuery("SELECT * FROM albums");
            return result.Select(MapAlbum).ToArray();
        }

        public Album Upsert(Album entity)
        {
            var result = this.ExecuteQuery($"SELECT * FROM albums WHERE id = {entity.Id}");

            if (result.Count() == 0)
                this.ExecuteQuery($"INSERT INTO albums (id, name, score, release_date, cover) VALUES ({entity.Id}, {entity.Name}, {entity.Score}, {entity.ReleaseDate}, {entity.Cover})");
            else if (result.Count() > 1)
                throw new Exception("More than one album with the same id");
            else 
                this.ExecuteQuery($"UPDATE albums SET name = {entity.Name}, score = {entity.Score}, release_date = {entity.ReleaseDate}, cover = {entity.Cover} WHERE id = {entity.Id}");
            
            var resultAfter = this.ExecuteQuery($"SELECT * FROM albums WHERE id = {entity.Id}");
            return MapAlbum(resultAfter.Single());
        }


        private static Album MapAlbum(IDictionary<string, object> entry)
        {
            return new Album()
            {
                Id = entry["id"].ToString(),
                Name = entry["name"].ToString(),
                Score = int.Parse(entry["score"].ToString()),
                ReleaseDate = entry["release_date"].ToString(),
                Cover = entry["cover"].ToString()
            };
        }
    }
}
