using System.Collections.Generic;

namespace Coolector.Services.Statistics.Shared.Dto
{
    public class UserStatisticsDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public RemarksCountStatisticsDto Remarks { get; set; }
        public IList<VoteDto> Votes { get; set; }
    }
}