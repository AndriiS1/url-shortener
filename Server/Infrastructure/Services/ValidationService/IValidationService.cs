using Domain.Models;
namespace Infrastructure.Services.ValidationService;

public interface IValidationService
{
    bool UserIsValid(User user);
}
