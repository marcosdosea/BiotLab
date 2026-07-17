using AutoMapper;
using BiotLabWeb.Models;
using Core;

namespace BiotLabWeb.Mapper
{
    public class ExperimentoProfile : Profile
    {
        public ExperimentoProfile()
        {
            CreateMap<Experimento, ExperimentoViewModel>()
                .ForMember(dest => dest.IdsPesquisadores,
                    opt => opt.MapFrom(src => src.ExperimentoPesquisadores.Select(ep => ep.IdPesquisador).ToList()))
                .ForMember(dest => dest.NomesPesquisadores,
                    opt => opt.MapFrom(src => src.ExperimentoPesquisadores.Select(ep => ep.IdPesquisadorNavigation.Nome).ToList()));

            CreateMap<ExperimentoViewModel, Experimento>()
                .ForMember(dest => dest.ExperimentoPesquisadores, opt => opt.Ignore())
                .ForMember(dest => dest.Gaiolas, opt => opt.Ignore())
                .ForMember(dest => dest.IdPesquisadorNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.Usoanestesicos, opt => opt.Ignore());
        }
    }
}
