using AutoMapper;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Shared.Dto;

namespace Coolector.Services.Statistics.Framework
{
    public class AutoMapperConfig
    {
        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserStatistics, UserStatisticsDto>();
            });

            return config.CreateMapper();
        }
    }
}