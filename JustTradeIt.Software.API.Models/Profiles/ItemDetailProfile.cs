using AutoMapper;
using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTradeIt.Software.API.Models.Profiles
{
    public class ItemDetailProfile : Profile
    {
        public ItemDetailProfile()
        {
            CreateMap<Item, ItemDetailsDto>()
              .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Tiltle))
              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
              .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ItemImages))
              .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => src.ItemCondition.Description));
            //Add the UserDTO

        }
    }
}
