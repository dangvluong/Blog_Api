using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repositories
{
    public abstract class BaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        protected void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        protected void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        protected IQueryable<T> FindAll(bool trackChanges)
        {
            return trackChanges ? _context.Set<T>() : _context.Set<T>().AsNoTracking();
        }
        protected IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {

            return trackChanges ? _context.Set<T>().Where(expression) : _context.Set<T>().AsNoTracking().Where(expression);
        }
        protected void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
