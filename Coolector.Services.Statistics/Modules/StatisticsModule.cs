using AutoMapper;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Repositories;
using Coolector.Services.Statistics.Shared.Dto;
using Coolector.Services.Statistics.Queries;

namespace Coolector.Services.Statistics.Modules
{
    public class StatisticsModule : ModuleBase
    {
        public StatisticsModule(IUserStatisticsRepository userStatisticsRepository,
            IMapper mapper) : base(mapper, "statistics")
        {
            Get("users", async args => await FetchCollection<BrowseUserStatistics, UserStatistics>
                (async x => await userStatisticsRepository.BrowseAsync(x))
                .MapTo<UserStatisticsDto>()
                .HandleAsync());

            Get("users/{id}", async args => await Fetch<GetUserStatistics, UserStatistics>
                (async x => await userStatisticsRepository.GetByIdAsync(x.Id))
                .MapTo<UserStatisticsDto>()
                .HandleAsync());
        }
    }
}