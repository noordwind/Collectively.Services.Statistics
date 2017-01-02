using AutoMapper;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Repositories;
using Coolector.Services.Statistics.Shared.Dto;
using Coolector.Services.Statistics.Queries;

namespace Coolector.Services.Statistics.Modules
{
    public class StatisticsModule : ModuleBase
    {
        public StatisticsModule(IRemarkStatisticsRepository remarkStatisticsRepository,
            IUserStatisticsRepository userStatisticsRepository,
            IMapper mapper) : base(mapper, "statistics")
        {
            Get("remarks", async args => await FetchCollection<BrowseRemarkStatistics, RemarkStatistics>
                (async x => await remarkStatisticsRepository.BrowseAsync(x))
                .MapTo<RemarkStatistics>()
                .HandleAsync());

            Get("remarks/{id}", async args => await Fetch<GetRemarkStatistics, RemarkStatistics>
                (async x => await remarkStatisticsRepository.GetAsync(x.Id))
                .MapTo<RemarkStatistics>()
                .HandleAsync());

            Get("remarks/general", async args => await Fetch<GetRemarkGeneralStatistics, RemarkGeneralStatistics>
                (async x => await remarkStatisticsRepository.GetGeneralStatisticsAsync(x))
                .MapTo<RemarkGeneralStatisticsDto>()
                .HandleAsync());

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