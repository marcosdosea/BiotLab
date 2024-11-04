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
                .ForMember(dest => dest.Fornecedor, opt => opt.MapFrom(src => src.IdFornecedorNavigation))
                .ForMember(dest => dest.Instituicao, opt => opt.MapFrom(src => src.IdInstituicaoNavigation));

            CreateMap<EntradumViewModel, Entradum>()
                .ForMember(dest => dest.IdFornecedorNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdInstituicaoNavigation, opt => opt.Ignore());
        }
    }
}
