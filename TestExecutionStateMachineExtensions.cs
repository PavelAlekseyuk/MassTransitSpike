using System;
using Automatonymous;
using Automatonymous.Binders;

namespace MassTransitSpike
{
  public static class TestExecutionStateMachineExtensions
  {
    /*public static EventActivityBinder<TestState, JobCompletedEventData> StoreJobMetadata(this EventActivityBinder<TestState, JobCompletedEventData> binder)
    {
      return binder.Then(context =>
      {
        // context.Instance.TestId = context.Data.TestId;
        context.Instance.AddIntoMetadataAndReturnLastTwoItems(context.Data.Metadata);
        Console.WriteLine($"{context.Instance.CorrelationId} Got metadata {context.Data.Metadata} for test {context.Instance.TestId}");
      });
    } */
  }
}
