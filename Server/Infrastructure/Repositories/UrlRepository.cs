using Domain.Dto;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UrlRepository : Repository<Url>, IUrlRepository
    {
        public UrlRepository(DbContext context) : base(context)
        {

        }

        public IEnumerable<TableUrlDataDto> GetAllTableUrls() 
        {
            return _context.Set<Url>().Select(e => new TableUrlDataDto() {OriginalUrl = e.OriginalUrl, ShortUrl=e.ShortUrl});
        }
    }
}
