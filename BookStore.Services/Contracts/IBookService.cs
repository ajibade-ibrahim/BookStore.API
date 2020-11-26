using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Domain.Entities.Dto;

namespace BookStore.Services.Contracts
{
    public interface IBookService
    {
        Task<IReadOnlyList<BookDto>> GetAllBooks();
    }
}