using MusicSmash.Database.Interfaces;
using MusicSmash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusicSmash.Models.Album;

namespace MusicSmash.PostgreSQL.Implemenations.Mediators
{
    public class AlbumMediator(IConnection connection) : MediatorDBTObject<Album, AlbumDB, long>(connection)
    {
        public override Album Mediate(AlbumDB entity)
        {
            if (entity is null)
                return Album.NotDefined;
            return new Album()
            {
                Id = entity.Id,
                Name = entity.Name,
                Cover = entity.Cover,
                Score = entity.Score
            };
        }
    }
}
