using AutoMapper;
using BiotLabWeb.Models;
using Core;

namespace BiotLabWeb.Mapper
{
    public class GaiolaharemProfile : Profile
    {
        public GaiolaharemProfile()
        {
            CreateMap<Gaiolaharem, GaiolaharemViewModel>()
                .ForMember(dest => dest.CodigoInternoGaiola,
                    opt => opt.MapFrom(src => src.IdGaiolaNavigation.CodigoInterno))
                .ForMember(dest => dest.CodigoInternoHarem,
                    opt => opt.MapFrom(src => src.IdHaremNavigation.CodigoInterno))
                .ForMember(dest => dest.NomePesquisador,
                    opt => opt.MapFrom(src => src.IdPesquisadorNavigation.Nome))
                .ForMember(dest => dest.NomeGaiola,
                    opt => opt.MapFrom(src => src.IdGaiolaNavigation.CodigoInterno))
                .ForMember(dest => dest.NomeHarem,
                    opt => opt.MapFrom(src => src.IdHaremNavigation.CodigoInterno))
                .ReverseMap()
                .ForMember(dest => dest.IdGaiolaNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdHaremNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdPesquisadorNavigation, opt => opt.Ignore());
        }
    }
}