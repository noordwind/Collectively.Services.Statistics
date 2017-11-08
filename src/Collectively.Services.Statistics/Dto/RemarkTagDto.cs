using System;

namespace Collectively.Services.Statistics.Dto
{
    public class RemarkTagDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid DefaultId { get; set; }
        public string Default { get; set; }           
    }
}