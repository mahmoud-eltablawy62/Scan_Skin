﻿using Microsoft.AspNetCore.Identity;
using ScanSkin.Core.Entites;
using ScanSkin.Core.Entites.Identity_User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanSkin.Core.Repo.Contract
{
    public interface IGenaric<T> where T : BaseClass
    {
        Task<T?> GetAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
