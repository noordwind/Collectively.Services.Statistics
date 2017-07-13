namespace Collectively.Services.Statistics.Domain
{
    public class RemarksCountStatistics
    {
        public int NewCount { get; protected set; }
        public int ReportedCount { get; protected set; }
        public int ProcessingCount { get; protected set; }
        public int ResolvedCount { get; protected set; }
        public int CanceledCount { get; protected set; }
        public int DeletedCount { get; protected set; }
        public int RenewedCount { get; protected set; }

        protected RemarksCountStatistics() { }

        public RemarksCountStatistics(int @new = 0, int reported = 0, 
                int processing = 0, int resolved = 0, int canceled = 0, 
                int deleted = 0, int renewed = 0)
        {
            NewCount = @new;
            ReportedCount = reported;
            ProcessingCount = processing;
            ResolvedCount = resolved;
            CanceledCount = canceled;
            DeletedCount = deleted;
            RenewedCount = renewed;
        }

        public virtual void IncreaseNew()
        {
            NewCount++;
        }

        public virtual void DecreaseNew()
        {
            NewCount--;
        }

        public virtual void IncreaseReported()
        {
            ReportedCount++;
        }

        public virtual void DecreaseReported()
        {
            ReportedCount--;
        }

        public virtual void IncreaseProcessing()
        {
            ProcessingCount++;
        }

        public virtual void DecreaseProcessing()
        {
            ProcessingCount--;
        }

        public virtual void IncreaseResolved()
        {
            ResolvedCount++;
        }

        public virtual void DecreaseResolved()
        {
            ResolvedCount--;
        }

        public virtual void IncreaseCanceled()
        {
            CanceledCount++;
        }

        public virtual void DecreaseCanceled()
        {
            CanceledCount--;
        }

        public virtual void IncreaseDeleted()
        {
            DeletedCount++;
        }

        public virtual void DecreaseDeleted()
        {
            DeletedCount--;
        }

        public virtual void IncreaseRenewed()
        {
            RenewedCount++;
        }

        public virtual void DecreaseRenewed()
        {
            RenewedCount--;
        }        
    }
}