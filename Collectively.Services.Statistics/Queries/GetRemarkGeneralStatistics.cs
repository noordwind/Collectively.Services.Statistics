using System;
using Collectively.Common.Queries;

namespace Collectively.Services.Statistics.Queries
{
    public class GetRemarkGeneralStatistics : IQuery
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}