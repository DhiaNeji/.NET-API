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
    public class TradeDetailsProfile : Profile
    {
        public TradeDetailsProfile()
        {
            CreateMap<Trade, TradeDetailsDto>()
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Id))
                //.ForMember(dest => dest.ReceivingItems, opt => opt.MapFrom(src => src))
                //.ForMember(dest => dest.OfferingItems, opt => opt.MapFrom(src => src.SendingtradeItems))
                .ForMember(dest => dest.ReceivedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.IssuedDate, opt => opt.MapFrom(src => src.IssueDate))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.TradeStatus));
                //Add the UserDTO
        }
    }
}
