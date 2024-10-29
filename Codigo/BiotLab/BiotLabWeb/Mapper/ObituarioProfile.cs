using AutoMapper;
using BiotLabWeb.Models;
using Core;

namespace BiotLabWeb.Mapper
{
    public class ObituarioProfile : Profile
    {
        public ObituarioProfile()
        {
            CreateMap<Obituario, ObituarioViewModel>().ReverseMap();
        }
    }
}