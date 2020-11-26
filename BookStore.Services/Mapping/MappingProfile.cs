using AutoMapper;
using BookStore.Domain.Entities;
using BookStore.Domain.Entities.Dto;

namespace BookStore.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Author, AuthorDto>().ReverseMap();
            CreateMap<Author, AuthorCreationDto>().ReverseMap();
            CreateMap<Author, AuthorUpdateDto>().ReverseMap();
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<Book, BookCreationDto>().ReverseMap();
        }
    }
}