﻿using System;
using System.Threading.Tasks;
using BookStore.Data.Repositories.Contracts;
using BookStore.Domain.Entities;

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

        public async Task DeleteAuthorAsync(Author author)
        {
            Delete(author);
            await SaveAsync();
        }
    }
}