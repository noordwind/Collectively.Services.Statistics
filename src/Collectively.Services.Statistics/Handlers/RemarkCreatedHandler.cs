using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Statistics.Repositories;
using Collectively.Common.ServiceClients;

namespace Collectively.Services.Statistics.Handlers
{
    public class RemarkCreatedHandler : RemarkStateChangedBaseHandler<RemarkCreated>
    {
        public RemarkCreatedHandler(IHandler handler, 
            IRemarkStatisticsRepository remarkStatisticsRepository,
            IUserStatisticsRepository userStatisticsRepository,
            ICategoryStatisticsRepository categoryStatisticsRepository,
            ITagStatisticsRepository tagStatisticsRepository,
            IServiceClient serviceClient) : 
                base(handler, remarkStatisticsRepository, 
                    userStatisticsRepository, categoryStatisticsRepository, 
                    tagStatisticsRepository, serviceClient)
        {
            Setup(x => { x.Remarks.IncreaseReported(); x.Remarks.IncreaseNew(); }, 
                  x => { x.Remarks.IncreaseReported(); x.Remarks.IncreaseNew(); }, 
                  x => { x.Remarks.IncreaseReported(); x.Remarks.IncreaseNew(); });
        }
    }
}