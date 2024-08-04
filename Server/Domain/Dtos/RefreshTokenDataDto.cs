namespace Domain.Dtos;

public class RefreshTokenDataDto
{
    public required string RefreshToken { get; init; }
    public required DateTime RefreshTokenExpiryTime { get; set; }
}
