using AutoMapper;
using BiotLabWeb.Models;
using Core;

namespace BiotLabWeb.Mapper

{
    public class BioterioProfile : Profile
    {
        public BioterioProfile()
        {
            CreateMap<Bioterio, BioterioViewModel>().ReverseMap();
        }
    }
}
