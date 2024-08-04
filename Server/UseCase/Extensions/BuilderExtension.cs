using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UseCase.Commands.Login;
namespace UseCase.Extensions;

public static class BuilderExtension
{
    public static void ConfigureUseCaseLayer(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<LoginCommand>());
    }
}
