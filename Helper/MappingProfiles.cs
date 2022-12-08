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
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<User, UserPostDto>();
            CreateMap<UserPostDto, User>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Photo, PhotoDto>();
            CreateMap<PhotoDto, Photo>(); 
            CreateMap<Like, LikeDto>();
            CreateMap<LikeDto, Like>();   
        }
    }
}

