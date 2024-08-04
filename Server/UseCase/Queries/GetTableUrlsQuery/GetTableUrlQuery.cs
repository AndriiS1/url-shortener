using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace UseCase.Queries.GetTableUrlsQuery;

public record GetTableUrlQuery(long? UserId) : IRequest<IActionResult>;
