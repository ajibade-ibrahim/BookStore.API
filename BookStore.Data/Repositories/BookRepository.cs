using System.Threading.Tasks;
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

        public async Task CreateBookAsync(Book book)
        {
            Add(book);
            await SaveAsync();
        }
    }
}