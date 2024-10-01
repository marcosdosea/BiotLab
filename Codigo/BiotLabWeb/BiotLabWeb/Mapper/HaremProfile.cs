using AutoMapper;
using BiotLabWeb.Models;
using core;

namespace BiotLabWeb.Mapper
{
    public class HaremProfile : Profile
    {
        public HaremProfile()
        {
            CreateMap<Harem, HaremViewModel>().ReverseMap();
        }
    }
}
