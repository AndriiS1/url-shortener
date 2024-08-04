using Domain;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace UseCase.Queries.GetUrlQuery;

public class GetUrlQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUrlQuery, IActionResult>
{
    public async Task<IActionResult> Handle(GetUrlQuery query, CancellationToken cancellationToken)
    {
        var foundUrl = await unitOfWork.Urls.GetUrlWithLoadedUserData(query.Id);

        if (foundUrl == null)
            return new BadRequestObjectResult("Url with this id is not found.");

        return new OkObjectResult(new UrlInfoDto
        {
            Id = foundUrl.Id,
            OriginalUrl = foundUrl.OriginalUrl,
            ShortUrl = foundUrl.ShortUrl,
            Date = foundUrl.Date,
            UserName = $"{foundUrl.User.FirstName} {foundUrl.User.SecondName}"
        });
    }
}
