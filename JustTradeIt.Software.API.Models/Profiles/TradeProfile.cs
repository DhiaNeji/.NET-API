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
    public class TradeProfile : Profile
    {
        public TradeProfile()
        {
            CreateMap<Trade, TradeDto>()
               .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.IssuedDate, opt => opt.MapFrom(src => src.IssueDate))
               .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
               .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.TradeStatus));
        }
    }
}
