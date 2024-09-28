using AutoMapper;
using BiotLabWeb.Models;
using core;

namespace BiotLabWeb.Mapper
{
    public class GaiolaProfile : Profile
    {
        public GaiolaProfile()
        {
            CreateMap<Gaiola, GaiolaViewModel>().ReverseMap();
        }
    }
}
