using Microsoft.EntityFrameworkCore;
using ScanSkin.Core.Entites;
using ScanSkin.Core.Repo.Contract;
using ScanSkin.Repo.IdentityUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanSkin.Repo
{
    public class GenaricRepo<T> : IGenaric<T> where T : BaseClass
    {
        private readonly ScanSkinContext _Context;
        public GenaricRepo(ScanSkinContext context)
        {
            _Context = context;
        }

        public async Task Add(T entity) =>
            await _Context.AddAsync(entity);
       
        public  void Delete(T entity) => 
             _Context.Remove(entity); 
        
        public async Task<IReadOnlyList<T>> GetAllAsync() =>
            await _Context.Set<T>().ToListAsync();

        public async Task<T?> GetAsync(int id) =>
            await _Context.Set<T>().FindAsync(id);
     
        
        public void Update(T entity) => 
            _Context.Update(entity);
        
    }
}
