using System;
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
        
        public async Task<TEntity> GetById(int id)
        {
            var result = await _context.Set<TEntity>().FirstOrDefaultAsync(obj => GetTEntityIdValue(obj) == id);
            if (result == null) throw new ArgumentNullException("Not Found!");

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

        // FIXME: temp
        private int GetTEntityIdValue(TEntity obj) 
        {
            var type = obj.GetType();
            var propertyName = type.Name + "ID";

            return (int)type.GetProperty(propertyName).GetValue(obj, null);
        }
    }
}