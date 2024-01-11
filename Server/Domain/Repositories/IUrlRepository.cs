using Domain.Dto;
using Domain.Models;

namespace Domain.Repositories
{
    public interface IUrlRepository : IRepository<Url>
    {
        IEnumerable<TableUrlDataDto> GetAllTableUrls();
    }
}
