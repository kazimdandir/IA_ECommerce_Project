using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Entities;
using AutoMapper;
using ECommerce.Services.DTO;

namespace ECommerce.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDTO>().ReverseMap();
            CreateMap<ShoppingCartItem, ShoppingCartItemDTO>().ReverseMap();
        }
    }
}
