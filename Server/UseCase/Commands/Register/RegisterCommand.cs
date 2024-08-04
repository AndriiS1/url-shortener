using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace UseCase.Commands.Register;

public record RegisterCommand(string Email, string Password) : IRequest<IActionResult>
{
    public long Id { get; set; }
    public required string FirstName { get; set; }
    public required string SecondName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
