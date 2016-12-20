using AutoMapper;
using Coolector.Common.Nancy;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Repositories;
using Coolector.Services.Statistics.Shared.Dto;
using Coolector.Services.Statistics.Shared.Queries;

namespace Coolector.Services.Statistics.Modules
{
    public class StatisticsModule : ModuleBase
    {
        public StatisticsModule(IReporterRepository reporterRepository,
            IResolverRepository resolverRepository,
            IMapper mapper) : base(mapper, "statistics")
        {
            Get("reporters", async args => await FetchCollection<BrowseReporters, Reporter>
                (async x => await reporterRepository.BrowseAsync(x))
                .MapTo<ReporterDto>()
                .HandleAsync());

            Get("resolvers", async args => await FetchCollection<BrowseResolvers, Resolver>
                (async x => await resolverRepository.BrowseAsync(x))
                .MapTo<ResolverDto>()
                .HandleAsync());
        }
    }
}