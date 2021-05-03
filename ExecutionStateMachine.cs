using System;
using Automatonymous;

namespace MassTransitSpike
{
  public class ExecutionStateMachine : MassTransitStateMachine<ExecutionState>
  {
    public ExecutionStateMachine()
    {
      // State
      InstanceState(x => x.CurrentState);

      // Events
      Event(() => ExecutionStarted, e => { });
      Event(() => ExecutionCompleted, e => { });

      // Behaviour
      Initially(
        When(ExecutionStarted)
          .Then(
            context =>
            {
              context.Instance.ExecutionId = context.Data.Id;
              Console.WriteLine($"{context.Instance.CorrelationId} Initially ExecutionStarted for execution {context.Instance.ExecutionId}");
            })
          .TransitionTo(Started),
        When(ExecutionCompleted)
          .Finalize()
      );

      During(
        Started,
        When(ExecutionCompleted)
          .Then(context => Console.WriteLine($"{context.Instance.CorrelationId} Got ExecutionCompleted for execution {context.Instance.ExecutionId}, so asking all related jobs to complete "))
          .Publish(context => new JobCancelledEventData(context.Instance.CorrelationId) )
          .TransitionTo(Completed),
        Ignore(ExecutionStarted)
      );

      During(
        Completed,
        Ignore(ExecutionStarted),
        Ignore(ExecutionCompleted)
      );

      SetCompletedWhenFinalized();
    }


    public State Started { get; private set; }
    public State Completed { get; private set; }

    public Event<ExecutionCompletedEventData> ExecutionCompleted { get; private set; }
    public Event<ExecutionStartedEventData> ExecutionStarted { get; private set; }
  }
}