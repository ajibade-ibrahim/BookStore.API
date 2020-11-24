using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.Data.Repositories.Contracts;
using BookStore.Domain.Entities.Dto;
using BookStore.Services.Contracts;

namespace BookStore.Services
{
    public class AuthorService : IAuthorService
    {
        public AuthorService(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public async Task<IReadOnlyList<AuthorDto>> GetAllAuthors()
        {
            var authors = await _authorRepository.GetAllAsync();
            return _mapper.Map<IReadOnlyList<AuthorDto>>(authors);
        }

        public async Task<AuthorDto> GetAuthor(Guid id)
        {
            ValidateId(id);
            var author = await _authorRepository.GetByIdAsync(id);

            return author == null ? null : _mapper.Map<AuthorDto>(author);
        }

        private static void ValidateId(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Empty id specified.", nameof(id));
            }
        }
    }
}