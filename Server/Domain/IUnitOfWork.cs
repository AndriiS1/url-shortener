using Domain.Repositories;

namespace Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IUrlRepository Urls { get; }
        int Complete();
    }
}
