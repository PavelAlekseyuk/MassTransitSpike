using System;
using MassTransit;

namespace MassTransitSpike
{
  public abstract class CorrelatedByStringId
  {
    private readonly Lazy<Guid> _lazyCorrelationId;

    protected CorrelatedByStringId(string id)
    {
      Id = id ?? throw new ArgumentNullException(nameof(id));
      _lazyCorrelationId = new Lazy<Guid>(() => GuidCreator.CreateGuidForString(id));  
    }

    public string Id { get; }

    public Guid CorrelationId => _lazyCorrelationId.Value;
  }
}