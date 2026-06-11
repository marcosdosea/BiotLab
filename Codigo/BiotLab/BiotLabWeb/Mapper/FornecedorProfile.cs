using AutoMapper;
using BiotLabWeb.Models;
using Core;

namespace BiotLabWeb.Mapper
{
    public class FornecedorProfile : Profile
    {
        public FornecedorProfile()
        {
            CreateMap<Fornecedor, FornecedorViewModel>()
                .ForMember(dest => dest.NomeInstituicao, opt => opt.MapFrom(src => src.IdInstituicaoNavigation.Nome));

            CreateMap<FornecedorViewModel, Fornecedor>()
                .ForMember(dest => dest.IdInstituicaoNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.Entrada, opt => opt.Ignore());
        }
    }
}