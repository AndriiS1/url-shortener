using Domain.Dtos;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories;

public class UrlRepository(DbContext context) : Repository<Url>(context), IUrlRepository
{
    private readonly DbContext _context = context;
    public IEnumerable<TableUrlDataDto> GetAllTableUrls()
    {
        return _context.Set<Url>().Select(e => new TableUrlDataDto
        {
            Id = e.Id,
            OriginalUrl = e.OriginalUrl,
            ShortUrl = e.ShortUrl
        });
    }

    public IEnumerable<TableUrlDataDto> GetAllTableUrlsWithDeleteCheck(long userId)
    {
        return _context.Set<Url>().Select(e => new TableUrlDataDto
        {
            Id = e.Id,
            OriginalUrl = e.OriginalUrl,
            ShortUrl = e.ShortUrl,
            CanDelete = e.UserId == userId
        });
    }

    public IEnumerable<TableUrlDataDto> GetAllAdminTableUrls()
    {
        return _context.Set<Url>().Select(e => new TableUrlDataDto
        {
            Id = e.Id,
            OriginalUrl = e.OriginalUrl,
            ShortUrl = e.ShortUrl,
            CanDelete = true
        });
    }

    public async Task<Url?> GetUrlWithLoadedUserData(long id)
    {
        return await _context.Set<Url>().Include(e => e.User).SingleOrDefaultAsync(e => e.Id == id);
    }
}
