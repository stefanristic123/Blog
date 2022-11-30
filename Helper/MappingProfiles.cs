 using System;
using System.Diagnostics.Metrics;
using AutoMapper;
using Blog.Dto;
using Blog.Models;

namespace Blog.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Post, PostDto>();
            CreateMap<PostDto, Post>();
            CreateMap<Writer, WriterDto>();
            CreateMap<WriterDto, Writer>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Photo, PhotoDto>();
            CreateMap<PhotoDto, Photo>();  
        }
    }
}

