using AutoMapper;
using BiotLabWeb.Models;
using Core;

namespace BiotLabWeb.Mapper
{
    public class ExperimentoProfile : Profile
    {
        public ExperimentoProfile()
        {
            CreateMap<Experimento, ExperimentoViewModel>().ReverseMap();
        }
    }
}