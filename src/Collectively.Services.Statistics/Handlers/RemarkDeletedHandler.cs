using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Statistics.Repositories;
using Collectively.Common.ServiceClients;

namespace Collectively.Services.Statistics.Handlers
{
    public class RemarkDeletedHandler : RemarkStateChangedBaseHandler<RemarkDeleted>
    {
        public RemarkDeletedHandler(IHandler handler, 
            IRemarkStatisticsRepository remarkStatisticsRepository,
            IUserStatisticsRepository userStatisticsRepository,
            ICategoryStatisticsRepository categoryStatisticsRepository,
            ITagStatisticsRepository tagStatisticsRepository,
            IServiceClient serviceClient) : 
                base(handler, remarkStatisticsRepository, 
                    userStatisticsRepository, categoryStatisticsRepository, 
                    tagStatisticsRepository, serviceClient)
        {
            Setup(x => { x.Remarks.IncreaseDeleted(); x.Remarks.DecreaseNew(); x.Remarks.DecreaseProcessing();
                         x.Remarks.DecreaseCanceled(); x.Remarks.DecreaseReported();}, 
                  x => { x.Remarks.IncreaseDeleted(); x.Remarks.DecreaseNew(); x.Remarks.DecreaseProcessing();
                         x.Remarks.DecreaseCanceled(); x.Remarks.DecreaseReported();}, 
                  x => { x.Remarks.IncreaseDeleted(); x.Remarks.DecreaseNew(); x.Remarks.DecreaseProcessing();
                         x.Remarks.DecreaseCanceled(); x.Remarks.DecreaseReported();});
        }
    }
}