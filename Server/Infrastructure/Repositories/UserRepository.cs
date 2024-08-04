using Domain.Dtos;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories;

public class UserRepository(DbContext context) : Repository<User>(context), IUserRepository
{
    private readonly DbContext _context = context;
    public async Task<bool> UpdateUserRefreshTokenData(long userId, RefreshTokenDataDto refreshTokenDataDto)
    {
        var userToUpdate = await _context.Set<User>().SingleOrDefaultAsync(u => u.Id == userId);

        if (userToUpdate is null)
            return false;

        userToUpdate.RefreshToken = refreshTokenDataDto.RefreshToken;
        userToUpdate.RefreshTokenExpiryTime = refreshTokenDataDto.RefreshTokenExpiryTime;
        return true;
    }
}
