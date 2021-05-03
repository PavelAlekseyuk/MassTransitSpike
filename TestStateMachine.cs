using System;
using System.Linq;
using Automatonymous;
using MassTransit;

namespace MassTransitSpike
{
  public class TestStateMachine : MassTransitStateMachine<TestState>
  {
    public TestStateMachine()
    {
      // State
      InstanceState(x => x.CurrentState);

      // Events
      Event(() => JobCompleted, e => { });
      Event(() => JobExpired, e => { });
      Event(() => JobCancelled, e => e.CorrelateBy((i, c) => i.ExecutionCorrelationId == c.Message.ExecutionId));

      Schedule(() => JobExpirationSchedule, i => i.ExpirationTokenId, i => i.Delay = TimeSpan.FromHours(24)); // cancellation in 24 hours

      // Behaviour
      Initially(
        When(JobCompleted)
          .Then(
            context =>
            {
              context.Instance.TestId = context.Data.TestId;
              context.Instance.ExecutionCorrelationId = context.Data.ExecutionId;

              var lastItems = context.Instance.AddIntoMetadataAndReturnLastTwoItems(context.Data.Metadata);
              Console.WriteLine($"{context.Instance.CorrelationId} Initially Got metadata {context.Data.Metadata} for test {context.Instance.TestId}");

              if (lastItems != null)
                Console.WriteLine($"{context.Instance.CorrelationId} Sending requests for {lastItems[0]} and {lastItems[1]}");
              else
                Console.WriteLine($"{context.Instance.CorrelationId} LastItems are null. Waiting...");
            })
          .Schedule(JobExpirationSchedule, i => new JobExpiredEventData(i.Data.TestId))
          .TransitionTo(Started),
        When(JobExpired)
          .Finalize()
      );

      During(
        Started,
        When(JobCompleted)
          .Then(
            context =>
            {
              context.Instance.TestId = context.Data.TestId;
              context.Instance.ExecutionCorrelationId = context.Data.ExecutionId;

              var lastItems = context.Instance.AddIntoMetadataAndReturnLastTwoItems(context.Data.Metadata);
              Console.WriteLine($"{context.Instance.CorrelationId} During Started Got metadata {context.Data.Metadata} for test {context.Instance.TestId}");

              if (lastItems != null)
                Console.WriteLine($"{context.Instance.CorrelationId} Sending requests for {lastItems[0]} and {lastItems[1]}");
              else
                Console.WriteLine($"{context.Instance.CorrelationId} LastItems are null. Waiting...");
            })
          .Schedule(JobExpirationSchedule, i => new JobExpiredEventData(i.Data.TestId)),
        When(JobExpired)
          .Then(context => Console.WriteLine($"{context.Instance.CorrelationId} Got expiration for test {context.Instance.TestId}"))
          .Unschedule(JobExpirationSchedule)
          .Finalize(),
        When(JobCancelled)
          .Then(context => Console.WriteLine($"{context.Instance.CorrelationId} Execution of {context.Instance.ExecutionCorrelationId} was cancelled"))
          .Unschedule(JobExpirationSchedule)
          .Finalize()
        );

      SetCompletedWhenFinalized();
    }


    public State Started { get; private set; }

    public Schedule<TestState, JobExpiredEventData> JobExpirationSchedule { get; private set; }

    public Event<JobCompletedEventData> JobCompleted { get; private set; }
    public Event<JobExpiredEventData> JobExpired { get; private set; } 
    public Event<JobCancelledEventData> JobCancelled { get; private set; }
  }
}