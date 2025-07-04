using GiftShop.Business.Abstract;
using GiftShop.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GiftShop.Business.Concrete
{
    public class Service<T> : IService<T> where T : class
    {
        internal DatabaseContext _context;
        internal DbSet<T> _dbSet;

        public Service(DatabaseContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public T Find(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> FindAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return _dbSet.FirstOrDefault(expression);
        }

        public List<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public List<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression).AsNoTracking().ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.Where(expression).AsNoTracking().ToListAsync();
        }

        public IQueryable<T> GetAllQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

      
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public int saveChanges()
        {
            throw new NotImplementedException();
        }

        public Task GetAsync(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }
}
