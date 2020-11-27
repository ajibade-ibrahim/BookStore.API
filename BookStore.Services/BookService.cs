using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.Data.Repositories.Contracts;
using BookStore.Domain.Entities;
using BookStore.Domain.Entities.Dto;
using BookStore.Domain.Exceptions;
using BookStore.Services.Contracts;

namespace BookStore.Services
{
    public class BookService : ServiceBase, IBookService
    {
        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public async Task<IReadOnlyList<BookDto>> GetAllBooksAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return _mapper.Map<IReadOnlyList<BookDto>>(books);
        }

        public async Task<BookDto> GetBookAsync(Guid id)
        {
            ValidateId(id);

            var book = await _bookRepository.GetByIdAsync(id);
            return book == null ? null : _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> CreateBookAsync(BookCreationDto bookCreationDto)
        {
            ValidateEntity(bookCreationDto);
            ValidateId(bookCreationDto.AuthorId);

            var authorExists = await _authorRepository.Exists(bookCreationDto.AuthorId);

            if (!authorExists)
            {
                throw new AuthorNotFoundException(bookCreationDto.AuthorId);
            }

            var book = _mapper.Map<Book>(bookCreationDto);
            await _bookRepository.CreateBookAsync(book);

            return _mapper.Map<BookDto>(book);
        }

        public async Task UpdateBookAsync(Guid id, BookUpdateDto bookUpdateDto)
        {
            ValidateId(id);
            ValidateEntity(bookUpdateDto);

            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
            {
                throw new BookNotFoundException(id);
            }

            _mapper.Map(bookUpdateDto, book);
            await _bookRepository.UpdateBookAsync(book);
        }

        public async Task DeleteBookAsync(Guid id)
        {
            ValidateId(id);

            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
            {
                throw new BookNotFoundException(id);
            }

            await _bookRepository.DeleteBookAsync(book);
        }
    }
}