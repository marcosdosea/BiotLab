using AutoMapper;
using BiotLabWeb.Models;
using Core;

namespace BiotLabWeb.Mapper
{
    public class AnestesicoProfile : Profile
    {
        public AnestesicoProfile()
        {
            CreateMap<Anestesico, AnestesicoViewModel>().ReverseMap();
        }
    }
}