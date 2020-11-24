using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.Data.Repositories.Contracts;
using BookStore.Domain.Entities;
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

        public async Task<AuthorDto> CreateAuthor(AuthorCreationDto authorCreationDto)
        {
            ValidateAuthorCreationDto(authorCreationDto);

            var author = _mapper.Map<Author>(authorCreationDto);
            await _authorRepository.CreateAuthorAsync(author);
            var authorDto = _mapper.Map<AuthorDto>(author);
            return authorDto;
        }

        private static void ValidateAuthorCreationDto(AuthorCreationDto authorCreationDto)
        {
            if (authorCreationDto == null)
            {
                throw new ArgumentException(nameof(authorCreationDto));
            }

            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(authorCreationDto);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(authorCreationDto, validationContext, validationResults, true))
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