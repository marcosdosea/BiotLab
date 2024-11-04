using AutoMapper;
using BiotLabWeb.Models;
using Core;

namespace BiotLabWeb.Mapper
{
    public class PesquisadorProfile : Profile
    {
        public PesquisadorProfile()
        {
            CreateMap<Pesquisador, PesquisadorViewModel>().ReverseMap();
        }
    }
}

