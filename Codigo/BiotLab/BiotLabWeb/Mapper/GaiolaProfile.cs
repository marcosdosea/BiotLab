using AutoMapper;
using BiotLabWeb.Models;
using Core;

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
