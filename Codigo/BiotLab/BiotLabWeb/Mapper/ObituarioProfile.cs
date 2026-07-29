using AutoMapper;
using BiotLabWeb.Models;
using Core;

namespace BiotLabWeb.Mapper
{
    public class ObituarioProfile : Profile
    {
        public ObituarioProfile()
        {
            CreateMap<Obituario, ObituarioViewModel>()
                .ForMember(destino => destino.NomePesquisador,
                    opcao => opcao.MapFrom(origem => origem.IdPesquisadorNavigation.Nome));

            CreateMap<ObituarioViewModel, Obituario>();
        }
    }
}