using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Automatonymous;
using MassTransit;
using MassTransit.Saga;
using MassTransit.Saga.InMemoryRepository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MassTransitSpike
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBus _bus;
        private readonly IndexedSagaDictionary<TestState> _sagaRepository;

        public Worker(
          ILogger<Worker> logger,
          IBus bus,
          IndexedSagaDictionary<TestState> sagaRepository)
        {
          _logger = logger;
          _bus = bus;
          _sagaRepository = sagaRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        { 
          Console.WriteLine("Start execution 1");
          var executionStarted = new ExecutionStartedEventData("execution1");
          await _bus.Publish(executionStarted, stoppingToken); 
          

          Console.WriteLine("Event 1");
          var testId = "1";
          
          await _bus.Publish(new JobCompletedEventData("1") { Metadata = "Job1", ExecutionId = executionStarted.CorrelationId }, stoppingToken); 
          await _bus.Publish(new JobCompletedEventData("2") { Metadata = "Job2", ExecutionId = executionStarted.CorrelationId }, stoppingToken);
          await _bus.Publish(new JobCompletedEventData("3") { Metadata = "Job3", ExecutionId = executionStarted.CorrelationId }, stoppingToken);

          await Task.Delay(1000, stoppingToken);
          Console.WriteLine("All items in repo");
          foreach (var item in _sagaRepository.Where(new SagaQuery<TestState>(x => true)))
          {
             Console.WriteLine($"{item.Instance.CorrelationId} test {item.Instance.CorrelationId}");
          } 

          await _bus.Publish(new ExecutionCompletedEventData("execution1"), stoppingToken); 

          //await Task.Delay(1000, stoppingToken);
          //await _bus.Publish(new JobExpiredEventData(testId), stoppingToken);

          await Task.Delay(1000, stoppingToken);
          Console.WriteLine("All items in repo after expiration");
          foreach (var item in _sagaRepository.Where(new SagaQuery<TestState>(x => true)))
          {
            Console.WriteLine($"{item.Instance.CorrelationId} test {item.Instance.CorrelationId}");
          } 

          // Console.WriteLine("Event 2");
          // await _bus.Publish(new SubmitTest { CorrelationId = "2", SomeDataSubmitted = $"Test 2 submitted" }, stoppingToken);

          // await _machine.RaiseEvent(new SubmitTest { CorrelationId = "1", SomeDataSubmitted = $"Test 1 submitted" }, _machine.SubmitOrder);

          /*await Task.Delay(1000);
          Console.WriteLine("Event 2");
          await _bus.Publish(new AcceptTest { CorrelationId = "1", SomeDataAccepted = $"Test 1 accepted" }, stoppingToken);*/


        }
    }
}
