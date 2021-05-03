using System;

namespace MassTransitSpike
{
  public class JobExpiredEventData : CorrelatedByStringId
  {
    public JobExpiredEventData(string testId)
      : base(testId)
    {
    }
  }

  public class JobCancelledEventData
  {
    public Guid ExecutionId { get; }

    public JobCancelledEventData(Guid executionId)
    {
      ExecutionId = executionId;
    }
  }
}