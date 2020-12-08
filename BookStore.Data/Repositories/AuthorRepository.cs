using System;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data.Repositories.Contracts;
using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(BookStoreDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <summary>
        /// Adds an author and calls SaveChanges on the underlying dbContext.
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public async Task CreateAuthorAsync(Author author)
        {
            Add(author);
            await SaveAsync();
        }

        /// <summary>
        /// Updates an author and calls SaveChanges on the underlying dbContext.
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public async Task UpdateAuthorAsync(Author author)
        {
            Update(author);
            await SaveAsync();
        }

        public async Task DeleteAuthorAsync(Guid id)
        {
            await Delete(id);
            await SaveAsync();
        }

        public Task<Author> GetAuthorWithBooksAsync(Guid id)
        {
            return _dbContext.Authors.Where(entity => entity.Id.Equals(id)).Include(entity => entity.Books).SingleAsync();
        }

        public async Task DeleteAuthorAsync(Author author)
        {
            Delete(author);
            await SaveAsync();
        }
    }
}