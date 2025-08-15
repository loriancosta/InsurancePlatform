using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using ProposalService.Domain.Interfaces;
using ProposalService.Infrastructure.Repositories;
using ProposalService.Infrastructure.Consumers;

namespace ProposalService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IProposalRepository, InMemoryProposalRepository>();
        
        services.AddMassTransit(x =>
        {
            x.AddConsumer<ProposalStatusRequestConsumer>();
            
            x.UsingInMemory((context, cfg) =>
            {
                cfg.ReceiveEndpoint("mq-proposal-status-request", e =>
                {
                    e.ConfigureConsumer<ProposalStatusRequestConsumer>(context);
                });
                
                cfg.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}
