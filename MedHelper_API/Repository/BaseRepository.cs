using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedHelper_API.Repository.Contracts;
using MedHelper_EF.Models;
using Microsoft.EntityFrameworkCore;

namespace MedHelper_API.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected MedHelperDB _context { get; set; }

        protected BaseRepository(MedHelperDB postgresDbContext)
        {
            _context = postgresDbContext;
        }
        
        // не працює (((
        public async Task<TEntity> GetById(int id)
        {
            Console.Write(id);
            // var result = await _context.Set<TEntity>(). FirstOrDefaultAsync(obj => obj.GetId() == id);
            var result = await _context.Set<TEntity>().Where(obj => obj.GetId() == id).FirstOrDefaultAsync();
            if (result == null) throw new KeyNotFoundException($"{typeof(TEntity)} hasn't been found.");

            return result;
        }

        public async Task Create(TEntity item)
        {
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(TEntity item)
        {
            _context.Remove(item);
            await _context.SaveChangesAsync();
        }
        
        public async Task Update(TEntity item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}