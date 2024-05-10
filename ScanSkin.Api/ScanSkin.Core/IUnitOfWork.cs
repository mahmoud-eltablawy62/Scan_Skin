using ScanSkin.Core.Entites;
using ScanSkin.Core.Repo.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanSkin.Core
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenaric<TEntity> Repo<TEntity>() where TEntity : BaseClass;
        Task<int> CompleteAsync();
    }
}
