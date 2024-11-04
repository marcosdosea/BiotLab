using AutoMapper;
using Core;
using BiotLabWeb.Models;

namespace BiotLabWeb.Mapper
{
    public class UsoanestesicoProfile : Profile
    {
        public UsoanestesicoProfile()
        {
            CreateMap<Usoanestesico, UsoanestesicoViewModel>().ReverseMap();
        }
    }
}
