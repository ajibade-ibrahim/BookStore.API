using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Domain.Entities.Dto;

namespace BookStore.Services.Contracts
{
    public interface IAuthorService
    {
        Task<IReadOnlyList<AuthorDto>> GetAllAuthors();
        Task<AuthorDto> GetAuthor(Guid id);
        Task<AuthorDto> CreateAuthor(AuthorCreationDto authorCreationDto);
        Task UpdateAuthor(AuthorUpdateDto authorUpdateDto);
    }
}