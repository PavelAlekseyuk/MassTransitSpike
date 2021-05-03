using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MassTransitSpike
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      return Host.CreateDefaultBuilder(args)
        .ConfigureServices(
          (hostContext, services) =>
          {
            services.AddMassTransit(
              x =>
              {
                x.AddSagaStateMachine<TestStateMachine, TestState>().InMemoryRepository();
                x.AddSagaStateMachine<ExecutionStateMachine, ExecutionState>().InMemoryRepository();

                x.UsingInMemory(
                  (context, cfg) =>
                  {
                    cfg.ConfigureEndpoints(context); 
                    cfg.UsePublishMessageScheduler();
                    cfg.UseInMemoryOutbox();

                    /*cfg.ReceiveEndpoint("test", e =>
                    {
                      e.UseInMemoryOutbox();

                      e.ConfigureSaga<TestState>(context);
                    });*/
                  });
              });

            services.AddMassTransitHostedService(true);
            services.AddHostedService<Worker>();

            /*var machine = new TestExecutionStateMachine();
            var repository = new InMemorySagaRepository<TestState>();

            var busControl = Bus.Factory.CreateUsingInMemory(cfg =>
            {    
              cfg.ReceiveEndpoint("order", e =>
              {
                e.StateMachineSaga(machine, repository);
              });
            });*/
          });
    }
  }
}
