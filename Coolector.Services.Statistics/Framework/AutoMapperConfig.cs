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
                cfg.CreateMap<RemarkStatistics, RemarkStatisticsDto>();
                cfg.CreateMap<RemarkStatistics.RemarkUser, RemarkStatisticsDto.UserDto>();
                cfg.CreateMap<RemarkStatistics.RemarkLocation, RemarkStatisticsDto.LocationDto>();
            });

            return config.CreateMapper();
        }
    }
}