using BookStore.Data.Repositories.Contracts;
using BookStore.Domain.Entities;

namespace BookStore.Data.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(BookStoreDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}