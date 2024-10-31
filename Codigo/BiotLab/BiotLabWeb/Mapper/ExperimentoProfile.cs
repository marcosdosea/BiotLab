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
                .ForMember(dest => dest.Gaiolas, opt => opt.MapFrom(src => string.Join(",", src.Gaiolas.Select(g => g.Id.ToString()))))
                .ReverseMap()
                .ForMember(dest => dest.Gaiolas, opt => opt.Ignore());
            CreateMap<ExperimentoViewModel, Experimento>()
                .ForMember(dest => dest.IdPesquisadorNavigation, opt => opt.MapFrom(src => new Pesquisador { Id = int.Parse(src.IdPesquisadorNavigation) }))
                .ReverseMap();
        }
    }
}