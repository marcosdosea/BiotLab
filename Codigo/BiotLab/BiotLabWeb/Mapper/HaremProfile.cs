using AutoMapper;
using BiotLabWeb.Models;
using Core;

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
