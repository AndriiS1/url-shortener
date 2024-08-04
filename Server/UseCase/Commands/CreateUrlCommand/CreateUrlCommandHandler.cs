using Domain;
using Domain.Models;
using Infrastructure.Services.UrlShortenerService;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace UseCase.Commands.CreateUrlCommand;

public class CreateUrlCommandHandler(IUnitOfWork unitOfWork, IUrlShortenerService urlShortenerService) : IRequestHandler<CreateUrlCommand, IActionResult>
{
    public async Task<IActionResult> Handle(CreateUrlCommand command,
        CancellationToken cancellationToken)
    {
        var tryOriginalUrl = await unitOfWork.Urls.SingleOrDefault(u => string.Compare(u.OriginalUrl, command.OriginalUrl) == 0);

        if (tryOriginalUrl != null)
            return new BadRequestObjectResult("This url is already shorten");

        var generatedShortUrl = "";
        var code = "";
        while (true)
        {
            var saltedPrefix = "";
            var serverAddress = $"{command.Scheme}://{command.Host}";
            code = urlShortenerService.GenerateShortUrl(command.OriginalUrl.ToLower() + saltedPrefix);
            generatedShortUrl = serverAddress + "/" + code;
            var tryFindShortUrl = await unitOfWork.Urls.SingleOrDefault(u => u.ShortUrl == generatedShortUrl);

            if (tryFindShortUrl == null)
                break;

            var random = new Random();
            saltedPrefix += random.Next(10).ToString();
        }

        var urlInstance = new Url
        {
            Date = DateTime.Now,
            OriginalUrl = command.OriginalUrl,
            UserId = command.UserId,
            Code = code,
            ShortUrl = generatedShortUrl
        };

        await unitOfWork.Urls.Add(urlInstance);
        await unitOfWork.Complete();
        return new OkResult();
    }
}
