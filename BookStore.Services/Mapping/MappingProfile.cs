﻿using AutoMapper;
using BookStore.Domain.Entities;
using BookStore.Domain.Entities.Dto;

namespace BookStore.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Author, AuthorDto>().ReverseMap();
            CreateMap<AuthorCreationDto, Author>();
            CreateMap<Book, BookDto>().ReverseMap();
        }
    }
}