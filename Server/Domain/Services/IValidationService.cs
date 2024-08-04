using Domain.Models;
namespace Domain.Services;

public interface IValidationService
{
    bool UserIsValid(User user);
}
