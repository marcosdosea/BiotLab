using AutoMapper;
using BiotLabWeb.Models;
using Core;

namespace BiotLabWeb.Mapper
{
    public class EntradaanestesicoProfile : Profile
    {
        public EntradaanestesicoProfile()
        {
            CreateMap<Entradaanestesico, EntradaanestesicoViewModel>();
               
        }
    }
}
