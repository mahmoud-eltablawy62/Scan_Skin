using ScanSkin.Core;
using ScanSkin.Core.Entites;
using ScanSkin.Core.Repo.Contract;
using ScanSkin.Repo.IdentityUser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanSkin.Repo
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ScanSkinContext context;

        private Hashtable _Repo;

        public UnitOfWork(ScanSkinContext _context)
        {
            context = _context;
            _Repo = new Hashtable();
        }
        public Task<int> CompleteAsync() =>
            context.SaveChangesAsync(); 
        

        public ValueTask DisposeAsync() => 
            context.DisposeAsync(); 
       

        public IGenaric<TEntity> Repo<TEntity>() where TEntity : BaseClass
        {
            var key = typeof(TEntity).Name;

            if (!_Repo.ContainsKey(key))
            {
                var repo = new GenaricRepo<TEntity>(context);

                _Repo.Add(key, repo);
            }
            return _Repo[key] as IGenaric<TEntity>;
        }
    }
}
