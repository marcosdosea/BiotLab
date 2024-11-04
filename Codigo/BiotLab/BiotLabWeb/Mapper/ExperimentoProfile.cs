using AutoMapper;
using BiotLabWeb.Models;
using Core;

public class ExperimentoProfile : Profile
{
    public ExperimentoProfile()
    {
        CreateMap<ExperimentoViewModel, Experimento>()
            .ForMember(dest => dest.Gaiolas, opt => opt.MapFrom(src => MapGaiolas(src.Gaiolas)))
            .ForMember(dest => dest.IdPesquisadorNavigation, opt => opt.MapFrom(src => new Pesquisador { Id = uint.Parse(src.IdPesquisadorNavigation ?? "0") }))
            .ForMember(dest => dest.Usoanestesicos, opt => opt.MapFrom(src => MapUsoanestesicos(src.Usoanestesicos)))
            .ReverseMap();
    }

    private ICollection<Gaiola> MapGaiolas(string? gaiolas)
    {
        if (string.IsNullOrEmpty(gaiolas))
            return new List<Gaiola>();

        var gaiolaIds = gaiolas.Split(',');
        var gaiolaList = new List<Gaiola>();

        foreach (var id in gaiolaIds)
        {
            if (uint.TryParse(id.Trim(), out uint gaiolaId))
            {
                gaiolaList.Add(new Gaiola { Id = gaiolaId });
            }
        }

        return gaiolaList;
    }

    private ICollection<Usoanestesico> MapUsoanestesicos(string? usoanestesicos)
    {
        if (string.IsNullOrEmpty(usoanestesicos))
            return new List<Usoanestesico>();

        var usoanestesicoIds = usoanestesicos.Split(',');
        var usoanestesicoList = new List<Usoanestesico>();

        foreach (var id in usoanestesicoIds)
        {
            if (uint.TryParse(id.Trim(), out uint usoanestesicoId))
            {
                usoanestesicoList.Add(new Usoanestesico { Id = usoanestesicoId }); 
            }
        }

        return usoanestesicoList;
    }
}
