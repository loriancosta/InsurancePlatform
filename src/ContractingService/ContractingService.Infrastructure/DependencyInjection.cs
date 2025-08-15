using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using ContractingService.Domain.Interfaces;
using ContractingService.Infrastructure.Repositories;
using ContractingService.Infrastructure.Clients;
using ProposalService.Application.Messages;

namespace ContractingService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IContractRepository, InMemoryContractRepository>();
        
        services.AddMassTransit(x =>
        {
            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
            
            x.AddRequestClient<ProposalStatusRequest>(new Uri("queue:mq-proposal-status-request"));
        });
        
        services.AddScoped<IProposalServiceClient, ProposalServiceMessageClient>();
        
        return services;
    }
}
