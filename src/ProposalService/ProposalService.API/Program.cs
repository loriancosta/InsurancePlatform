using FluentValidation;
using ProposalService.Infrastructure;

namespace ProposalService.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ProposalService.Application.Commands.CreateProposalCommand).Assembly));
        builder.Services.AddValidatorsFromAssembly(typeof(ProposalService.Application.Commands.CreateProposalCommand).Assembly);
        
        builder.Services.AddInfrastructure();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
