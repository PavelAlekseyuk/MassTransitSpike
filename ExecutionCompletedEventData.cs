namespace MassTransitSpike
{
  public class ExecutionCompletedEventData : CorrelatedByStringId
  {
    public ExecutionCompletedEventData(string id)
      : base(id)
    {
    }
  }
}