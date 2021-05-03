using System;
using MassTransit;

namespace MassTransitSpike
{
  public class JobCompletedEventData : CorrelatedByStringId
  {
    public JobCompletedEventData(string testId)
      : base(testId)
    {
    }

    public string TestId => base.Id;

    public string Metadata { get; set; } 
    
    public Guid ExecutionId { get; set; }
  }
}