using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Statistics.Repositories;
using Collectively.Common.ServiceClients;

namespace Collectively.Services.Statistics.Handlers
{
    public class RemarkRenewedHandler : RemarkStateChangedBaseHandler<RemarkRenewed>
    {
        public RemarkRenewedHandler(IHandler handler, 
            IRemarkStatisticsRepository remarkStatisticsRepository,
            IUserStatisticsRepository userStatisticsRepository,
            ICategoryStatisticsRepository categoryStatisticsRepository,
            ITagStatisticsRepository tagStatisticsRepository,
            IServiceClient serviceClient) : 
                base(handler, remarkStatisticsRepository, 
                    userStatisticsRepository, categoryStatisticsRepository, 
                    tagStatisticsRepository, serviceClient)
        {
            Setup(x => { x.Remarks.IncreaseRenewed(); x.Remarks.IncreaseNew(); x.Remarks.DecreaseCanceled(); }, 
                  x => { x.Remarks.IncreaseRenewed(); x.Remarks.IncreaseNew(); x.Remarks.DecreaseCanceled(); }, 
                  x => { x.Remarks.IncreaseRenewed(); x.Remarks.IncreaseNew(); x.Remarks.DecreaseCanceled(); });
        }
    }
}