using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.Data.Repositories.Contracts;
using BookStore.Domain.Entities.Dto;
using BookStore.Services.Contracts;

namespace BookStore.Services
{
    public class BookService : IBookService
    {
        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public async Task<IReadOnlyList<BookDto>> GetAllBooks()
        {
            var books = await _bookRepository.GetAllAsync();
            return _mapper.Map<IReadOnlyList<BookDto>>(books);
        }

        public async Task<BookDto> GetBook(Guid id)
        {
            ValidateBookId(id);

            var book = await _bookRepository.GetByIdAsync(id);
            return book == null ? null : _mapper.Map<BookDto>(book);
        }

        private static void ValidateBookId(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid identifier provided.", nameof(id));
            }
        }
    }
}