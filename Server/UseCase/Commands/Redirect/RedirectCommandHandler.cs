using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace UseCase.Commands.Redirect;

public class RedirectCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RedirectCommand, IActionResult>
{
    public async Task<IActionResult> Handle(RedirectCommand command,
        CancellationToken cancellationToken)
    {
        var foundUrl = await unitOfWork.Urls.SingleOrDefault(u => u.Code == command.code);

        return foundUrl is null ? new NotFoundResult() : new RedirectResult(foundUrl.OriginalUrl);
    }
}
