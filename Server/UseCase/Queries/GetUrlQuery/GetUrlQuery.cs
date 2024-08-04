using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace UseCase.Queries.GetUrlQuery;

public record GetUrlQuery(long Id) : IRequest<IActionResult>;
