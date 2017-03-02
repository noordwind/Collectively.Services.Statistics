using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Collectively.Services.Statistics.Domain;
using Collectively.Services.Statistics.Dto;
using Collectively.Services.Statistics.Repositories;
using Collectively.Services.Statistics.Queries;

namespace Collectively.Services.Statistics.Modules
{
    public class StatisticsModule : ModuleBase
    {
        public StatisticsModule(IRemarkStatisticsRepository remarkStatisticsRepository,
            IUserStatisticsRepository userStatisticsRepository,
            ICategoryStatisticsRepository categoryStatisticsRepository,
            ITagStatisticsRepository tagStatisticsRepository,
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

            Get("remarks/general", async args => await Fetch<GetRemarkGeneralStatistics, RemarksCountStatistics>
                (async x => await remarkStatisticsRepository.GetGeneralStatisticsAsync(x))
                .MapTo<RemarksCountStatisticsDto>()
                .HandleAsync());

            Get("categories", async args => await FetchCollection<BrowseCategoryStatistics, CategoryStatistics>
                (async x => await categoryStatisticsRepository.BrowseAsync(x))
                .MapTo<CategoryStatisticsDto>()
                .HandleAsync());

            Get("tags", async args => await FetchCollection<BrowseTagStatistics, TagStatistics>
                (async x => await tagStatisticsRepository.BrowseAsync(x))
                .MapTo<TagStatisticsDto>()
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