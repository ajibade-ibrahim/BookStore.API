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
    }
}