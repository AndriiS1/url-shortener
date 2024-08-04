using Domain.Dtos;
using Domain.Models;
namespace Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<bool> UpdateUserRefreshTokenData(long userId, RefreshTokenDataDto refreshTokenDataDto);
}
