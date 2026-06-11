using AutoMapper;
using BiotLabWeb.Models;
using Core;

namespace BiotLabWeb.Mapper
{
    public class UsoanestesicoProfile : Profile
    {
        public UsoanestesicoProfile()
        {
            CreateMap<Usoanestesico, UsoanestesicoViewModel>()
                .ForMember(dest => dest.NomePesquisador,
                    opt => opt.MapFrom(src => src.IdPesquisadorNavigation.Nome))
                .ForMember(dest => dest.NomeExperimento,
                    opt => opt.MapFrom(src => src.IdExperimentoNavigation.Cepa))
                .ForMember(dest => dest.NomeAnestesico,
                    opt => opt.MapFrom(src => src.Entradaanestesico.IdAnestesicoNavigation.Nome))
                .ForMember(dest => dest.Lote,
                    opt => opt.MapFrom(src => src.Entradaanestesico.Lote))
                .ForMember(dest => dest.DataEntrada,
                    opt => opt.MapFrom(src => src.Entradaanestesico.IdEntradaNavigation.DataEntrada));

            CreateMap<UsoanestesicoViewModel, Usoanestesico>()
                .ForMember(dest => dest.Entradaanestesico, opt => opt.Ignore())
                .ForMember(dest => dest.IdExperimentoNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdPesquisadorNavigation, opt => opt.Ignore());
        }
    }
}