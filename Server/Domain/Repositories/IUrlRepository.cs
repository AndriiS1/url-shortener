using Domain.Dtos;
using Domain.Models;
namespace Domain.Repositories;

public interface IUrlRepository : IRepository<Url>
{
    IEnumerable<TableUrlDataDto> GetAllTableUrls();
    IEnumerable<TableUrlDataDto> GetAllTableUrlsWithDeleteCheck(long userId);
    IEnumerable<TableUrlDataDto> GetAllAdminTableUrls();
    Task<Url?> GetUrlWithLoadedUserData(long id);
}
