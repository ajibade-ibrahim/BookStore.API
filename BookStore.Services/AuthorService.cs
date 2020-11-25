using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SystemValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using System.Text;
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

        public async Task<AuthorDto> CreateAuthor(AuthorCreationDto authorCreationDto)
        {
            ValidateAuthor(authorCreationDto);

            var author = _mapper.Map<Author>(authorCreationDto);
            await _authorRepository.CreateAuthorAsync(author);
            var authorDto = _mapper.Map<AuthorDto>(author);
            return authorDto;
        }

        public async Task UpdateAuthor(Guid id, AuthorUpdateDto authorUpdateDto)
        {
            ValidateId(id);
            ValidateAuthor(authorUpdateDto);

            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                throw new AuthorNotFoundException(id);
            }

            _mapper.Map(authorUpdateDto, author);
            await _authorRepository.SaveAsync();
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

            ValidateAuthor(authorUpdateDto);

            _mapper.Map(authorUpdateDto, author);
            await _authorRepository.SaveAsync();
        }

        private static void ValidateAuthor<T>(T author) where T : class
        {
            if (author == null)
            {
                throw new ArgumentException(nameof(author));
            }

            var validationContext = new SystemValidationContext(author);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(author, validationContext, validationResults, true))
            {
                var builder = new StringBuilder();

                foreach (var result in validationResults)
                {
                    builder.AppendLine($"{string.Join(',', result.MemberNames)} : {string.Join(',', result.ErrorMessage)}");
                }

                throw new ValidationException($"Author failed validation. {Environment.NewLine} {builder}");
            }
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