using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Core.Entities;
using ECommerce.WebAPI.ViewModels;

namespace ECommerce.WebAPI.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMappings();
        }

        private void CreateMappings()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

            CreateMap<Order, OrderViewModel>()
                .ForMember(o => o.OrderId, opt => opt.MapFrom(o => o.Id))
                .ReverseMap();


            CreateMap<OrderItem, OrderItemViewModel>()
                .ForMember(o => o.CategoryName, opt => opt.MapFrom(src => src.Product.Category.CategoryName))
                .ForMember(o => o.ProductTitle, opt => opt.MapFrom(src => src.Product.Title))
                .ReverseMap();


            CreateMap<OrderCreateModel, Order>()
                .ReverseMap();

            CreateMap<OrderItemCreateModel, OrderItem>()
                .ReverseMap();


            CreateMap<Review, ReviewViewModel>()
                .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product.Title))
                .ForMember(dest => dest.ReviewerFullName, opt => opt.MapFrom(src => src.User.FullName))
                .ReverseMap();

            CreateMap<ReviewCreateModel, Review>()
                .ReverseMap();
        }
    }
}
