using MusicSmash.Database.Interfaces;
using MusicSmash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusicSmash.Models.Album;
using static MusicSmash.Models.Game;

namespace MusicSmash.PostgreSQL.Implemenations.Mediators
{
    public class GameMediator(IConnection connection) : MediatorDBTObject<Game, GameDB, long>(connection)
    {
        public override Game Mediate(GameDB entity)
        {
            var albumRepository = connection.Detach<Album, AlbumDB, long>();
            var albumMediator = new AlbumMediator(connection);

            var left = albumRepository.Get(entity.Left);
            var right = albumRepository.Get(entity.Right);
            var winner = entity.Winner is null ? null : albumRepository.Get(entity.Winner ?? -1);

            return new Game()
            {
                Id = entity.Id,
                Winner = winner is null ? Album.NotDefined : albumMediator.Mediate(winner),
                Left = albumMediator.Mediate(left),
                Right = albumMediator.Mediate(right)
            };
        }
    }
}
