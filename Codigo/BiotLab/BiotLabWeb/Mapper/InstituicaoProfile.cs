using AutoMapper;
using BiotLabWeb.Models;
using Core;

namespace BiotLabWeb.Mapper
{
    public class InstituicaoProfile : Profile
    {
        public InstituicaoProfile()
        {
            CreateMap<Instituicao, InstituicaoViewModel>().ReverseMap();
        }
    }
}