using Microsoft.Extensions.DependencyInjection;
using UseCase.Commands.Login;
namespace UseCase;

public class BuilderExtension
{
    public void ConfigureUseCaseLayer(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<LoginCommand>());
    }
}
