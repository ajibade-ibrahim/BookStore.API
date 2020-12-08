using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.Data.Repositories.Contracts;
using BookStore.Domain.Entities;
using BookStore.Domain.Entities.Dto;
using BookStore.Domain.Exceptions;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.JsonPatch;

namespace BookStore.Services
{
    public class AuthorService : ServiceBase, IAuthorService
    {
        public AuthorService(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public async Task<AuthorDto> CreateAuthor(AuthorCreationDto authorCreationDto)
        {
            ValidateEntity(authorCreationDto);

            var author = _mapper.Map<Author>(authorCreationDto);
            await _authorRepository.CreateAuthorAsync(author);
            return _mapper.Map<AuthorDto>(author);
        }

        public async Task DeleteAuthor(Guid id)
        {
            ValidateId(id);

            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                throw new AuthorNotFoundException(id);
            }

            await _authorRepository.DeleteAuthorAsync(id);
        }

        public async Task<IReadOnlyList<AuthorDto>> GetAllAuthors()
        {
            var authors = await _authorRepository.GetAllAsync();
            return _mapper.Map<IReadOnlyList<AuthorDto>>(authors);
        }

        public async Task<AuthorDto> GetAuthor(Guid id)
        {
            ValidateId(id);

            var author = await _authorRepository.GetAuthorWithBooksAsync(id);
            return author == null ? null : _mapper.Map<AuthorDto>(author);
        }

        public async Task PatchAuthor(Guid id, JsonPatchDocument<AuthorUpdateDto> jsonPatchDocument)
        {
            ValidateId(id);
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                throw new AuthorNotFoundException(id);
            }

            var authorUpdateDto = _mapper.Map<AuthorUpdateDto>(author);
            jsonPatchDocument.ApplyTo(authorUpdateDto);

            ValidateEntity(authorUpdateDto);

            _mapper.Map(authorUpdateDto, author);
            await _authorRepository.SaveAsync();
        }

        public async Task UpdateAuthor(Guid id, AuthorUpdateDto authorUpdateDto)
        {
            ValidateId(id);
            ValidateEntity(authorUpdateDto);

            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                throw new AuthorNotFoundException(id);
            }

            _mapper.Map(authorUpdateDto, author);
            await _authorRepository.SaveAsync();
        }
    }
}