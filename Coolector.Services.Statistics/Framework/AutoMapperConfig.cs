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
                cfg.CreateMap<Reporter, ReporterDto>()
                    .ForMember(x => x.Name, m => m.MapFrom(s => s.User.Name))
                    .ForMember(x => x.Count, m => m.MapFrom(s => s.ReportedCount));
                cfg.CreateMap<Resolver, ResolverDto>()
                    .ForMember(x => x.Name, m => m.MapFrom(s => s.User.Name))
                    .ForMember(x => x.Count, m => m.MapFrom(s => s.ResolvedCount));
            });

            return config.CreateMapper();
        }
    }
}