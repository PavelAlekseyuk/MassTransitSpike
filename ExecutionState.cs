using System;
using Automatonymous;

namespace MassTransitSpike
{
  public class ExecutionState : SagaStateMachineInstance
  {
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } 
    public string ExecutionId { get; set; }
  }
}