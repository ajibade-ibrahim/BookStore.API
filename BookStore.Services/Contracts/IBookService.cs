using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Domain.Entities.Dto;

namespace BookStore.Services.Contracts
{
    public interface IBookService
    {
        Task<IReadOnlyList<BookDto>> GetAllBooksAsync();
        Task<BookDto> GetBookAsync(Guid id);
        Task<BookDto> CreateBookAsync(BookCreationDto bookCreationDto);
        Task UpdateBookAsync(Guid id, BookUpdateDto bookUpdateDto);
    }
}