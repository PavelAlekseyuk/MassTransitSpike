using System;

namespace MassTransitSpike
{
  public class ExecutionStartedEventData
  {
    private readonly Lazy<Guid> _lazyCorrelationId;

    public ExecutionStartedEventData(string id)
    {
      Id = id ?? throw new ArgumentNullException(nameof(id));
      _lazyCorrelationId = new Lazy<Guid>(() => GuidCreator.CreateGuidForString(id));  
    }

    public string Id { get; }

    public Guid CorrelationId => _lazyCorrelationId.Value;

  }
}