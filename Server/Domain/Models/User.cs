using Domain.Enums;
namespace Domain.Models;

public class User
{
    public long Id { get; set; }
    public required string FirstName { get; set; }
    public required string SecondName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string RefreshToken { get; set; }
    public UserRole Role { get; set; } = UserRole.Basic;
    public required DateTime RefreshTokenExpiryTime { get; set; }
    public ICollection<Url> Urls { get; } = new List<Url>();
}
