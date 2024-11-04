using AutoMapper;
using BiotLabWeb.Models;
using Core;

namespace BiotLabWeb.Mapper
{
    public class EntradaanestesicoProfile : Profile
    {
        public EntradaanestesicoProfile()
        {
            CreateMap<Entradaanestesico, EntradaanestesicoViewModel>()
                .ForMember(dest => dest.NomeAnestesico, opt => opt.MapFrom(src => src.IdAnestesicoNavigation.Nome))
                .ForMember(dest => dest.DataEntrada, opt => opt.MapFrom(src => src.IdEntradaNavigation.DataEntrada));

            CreateMap<EntradaanestesicoViewModel, Entradaanestesico>()
                .ForMember(dest => dest.IdAnestesicoNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdEntradaNavigation, opt => opt.Ignore());
        }
    }
}
