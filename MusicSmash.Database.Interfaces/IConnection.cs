using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSmash.Database.Interfaces
{
    public interface IConnection: IDisposable
    {
        IRepository<T> Detach<T>();
    }
}
