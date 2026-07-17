using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace BiotLabWeb.Controllers.Tests
{
    internal static class MapperTestFactory
    {
        public static MapperConfiguration CreateConfiguration(Action<IMapperConfigurationExpression> configure)
        {
            return new MapperConfiguration(configure, NullLoggerFactory.Instance);
        }

        public static IMapper CreateMapper(Profile profile)
        {
            return CreateConfiguration(cfg => cfg.AddProfile(profile)).CreateMapper();
        }
    }
}
