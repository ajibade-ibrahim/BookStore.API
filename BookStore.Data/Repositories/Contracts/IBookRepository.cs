using BookStore.Domain.Entities;

namespace BookStore.Data.Repositories.Contracts
{
    public interface IBookRepository : IRepository<Book>
    {
    }
}