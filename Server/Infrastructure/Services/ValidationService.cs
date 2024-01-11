using Domain.Models;
using Domain.Services;
using System.Text.RegularExpressions;

namespace Infrastructure.Services
{
    public class ValidationService : IValidationService
    {
        const string namePattern = "^[a-zA-Z0-9]*$";
        const string emailPattern = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
        const string passwordPattern = "^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$";
        public bool UserIsValid(User user)
        {
            if (!Regex.IsMatch(user.FirstName ?? "", namePattern))
            {
                return false;
            }
            else if (!Regex.IsMatch(user.SecondName ?? "", namePattern))
            {
                return false;
            }
            else if (!Regex.IsMatch(user.Email ?? "", emailPattern))
            {
                return false;
            }
            else if (!Regex.IsMatch(user.Password ?? "", passwordPattern))
            {
                return false;
            }
            return true;
        }
    }
}
