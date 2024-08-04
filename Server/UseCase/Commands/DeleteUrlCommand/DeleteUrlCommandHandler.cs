using Domain;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace UseCase.Commands.DeleteUrlCommand;

public class DeleteUrlCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUrlCommand, IActionResult>
{
    public async Task<IActionResult> Handle(DeleteUrlCommand command, CancellationToken cancellationToken)
    {
        var foundUrl = await unitOfWork.Urls.SingleOrDefault(e => e.Id == command.LinkId);

        if (foundUrl == null)
            return new BadRequestObjectResult("Url with this id is not found.");

        var foundUser = await unitOfWork.Users.SingleOrDefault(e => e.Id == command.UserId);

        if (foundUrl.UserId != command.UserId && foundUser?.Role != UserRole.Admin) return new BadRequestObjectResult("No permission.");

        unitOfWork.Urls.Remove(foundUrl);
        await unitOfWork.Complete();

        return new NoContentResult();
    }
}
