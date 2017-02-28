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
                cfg.CreateMap<User, RemarkStatisticsDto.UserDto>();
                cfg.CreateMap<Location, RemarkStatisticsDto.LocationDto>();
                cfg.CreateMap<RemarksCountStatistics, RemarkGeneralStatisticsDto>();
                cfg.CreateMap<CategoryStatistics, CategoryStatisticsDto>();
                cfg.CreateMap<TagStatistics, TagStatisticsDto>();
            });

            return config.CreateMapper();
        }
    }
}