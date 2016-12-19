using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Services;
using Coolector.Services.Remarks.Shared.Events;
using Coolector.Services.Statistics.Domain;
using Coolector.Services.Statistics.Repositories;

namespace Coolector.Services.Statistics.Handlers
{
    public class RemarkResolvedHandler : IEventHandler<RemarkResolved>
    {
        private readonly IHandler _handler;
        private readonly IResolverRepository _repository;

        public RemarkResolvedHandler(IHandler handler, IResolverRepository repository)
        {
            _handler = handler;
            _repository = repository;
        }

        public async Task HandleAsync(RemarkResolved @event)
        {
            await _handler
                .Run(async () =>
                {
                    var resolver = await _repository.GetByNameAsync(@event.Username);
                    if (resolver.HasNoValue)
                        resolver = new Resolver(@event.UserId, @event.Username);
                    else
                        resolver.Value.IncreaseResolvedCount();

                    await _repository.UpsertAsync(resolver.Value);
                })
                .OnError((ex, logger) => logger.Error(ex, $"Error while handling {typeof(RemarkResolved).Name} event"))
                .ExecuteAsync();
        }
    }
}