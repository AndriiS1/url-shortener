using Domain;
using Domain.Enums;
using Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace UseCase.Queries.GetTableUrlsQuery;

public class GetTableUrlsQueryHandler(IUnitOfWork unitOfWork, IUrlShortenerService urlShortenerService) : IRequestHandler<GetTableUrlQuery, IActionResult>
{
    public async Task<IActionResult> Handle(GetTableUrlQuery query,
        CancellationToken cancellationToken)
    {
        if (query.UserId == null)
            return new ObjectResult(unitOfWork.Urls.GetAllTableUrls().ToList());

        var foundUser = await unitOfWork.Users.SingleOrDefault(e => e.Id == query.UserId.Value);

        return foundUser?.Role == UserRole.Admin ? new OkObjectResult(unitOfWork.Urls.GetAllAdminTableUrls().ToList())
            : new OkObjectResult(unitOfWork.Urls.GetAllTableUrlsWithDeleteCheck(query.UserId.Value).ToList());
    }
}
