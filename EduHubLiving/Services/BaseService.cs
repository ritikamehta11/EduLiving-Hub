using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using EduHubLiving.Models;

namespace EduHubLiving.Services
{
    public class BaseService<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;

        public BaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}