using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BookStoreDbContext _dbContext;
        private readonly DbSet<T> _table;

        public Repository(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
            _table = dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            _table.Add(entity);
        }

        public async Task Delete(Guid id)
        {
            _table.Remove(await _table.FindAsync(id));
        }

        public void Delete(T entity)
        {
            _table.Remove(entity);
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _table.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _table.FindAsync(id);
        }

        public void Update(T entity)
        {
            _table.Update(entity);
        }
    }
}