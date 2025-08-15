
using FluentValidation;
using ContractingService.Infrastructure;

namespace ContractingService.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ContractingService.Application.Commands.CreateContractCommand).Assembly));
        builder.Services.AddValidatorsFromAssembly(typeof(ContractingService.Application.Commands.CreateContractCommand).Assembly);
        
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
