using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Automatonymous;

namespace MassTransitSpike
{
  public class TestState : SagaStateMachineInstance
  {
    public TestState()
    {
      _metadataCollection = new List<string>();
    }

    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    
    public Guid? ExpirationTokenId { get; set; }
    
    public string TestId { get; set; }
    public Guid ExecutionCorrelationId { get; set; }


    private readonly List<string> _metadataCollection;

    public IReadOnlyList<string> MetadataCollection
    {
      get
      {
        lock (_metadataCollection)
          return _metadataCollection.ToArray();
      }
    }

    public string[] AddIntoMetadataAndReturnLastTwoItems(string metadata)
    {
      lock (_metadataCollection)
      {
        _metadataCollection.Add(metadata);

        if (_metadataCollection.Count < 2)
          return null;

        return _metadataCollection.TakeLast(2).ToArray();
      }
    }
  }
}