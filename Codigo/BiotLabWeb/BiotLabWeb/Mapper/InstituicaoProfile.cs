using AutoMapper;
using BiotLabWeb.Models;
using core;

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