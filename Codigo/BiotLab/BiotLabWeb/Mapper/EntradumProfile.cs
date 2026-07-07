using AutoMapper;
using BiotLabWeb.Models;
using Core;

namespace BiotLabWeb.Mapper
{
    public class EntradumProfile : Profile
    {
        public EntradumProfile()
        {
            CreateMap<Entradum, EntradumViewModel>()
                .ForMember(dest => dest.NomeFornecedor, opt => opt.MapFrom(src => src.IdFornecedorNavigation.Nome))
                .ForMember(dest => dest.NomeInstituicao, opt => opt.MapFrom(src => src.IdInstituicaoNavigation.Nome));

            CreateMap<EntradumViewModel, Entradum>()
                .ForMember(dest => dest.IdFornecedorNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdInstituicaoNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.Entradaanestesicos, opt => opt.Ignore());
        }
    }
}