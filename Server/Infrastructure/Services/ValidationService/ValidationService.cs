using System.Text.RegularExpressions;
using Domain.Models;
namespace Infrastructure.Services.ValidationService;

public class ValidationService : IValidationService
{
    private const string NamePattern = "^[a-zA-Z0-9]*$";
    private const string EmailPattern = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
    private const string PasswordPattern = "^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$";
    public bool UserIsValid(User user)
    {
        if (!Regex.IsMatch(user.FirstName ?? "", NamePattern))
        {
            return false;
        }
        if (!Regex.IsMatch(user.SecondName ?? "", NamePattern))
        {
            return false;
        }
        if (!Regex.IsMatch(user.Email ?? "", EmailPattern))
        {
            return false;
        }
        if (!Regex.IsMatch(user.Password ?? "", PasswordPattern))
        {
            return false;
        }
        return true;
    }
}
