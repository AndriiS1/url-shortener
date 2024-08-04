using Domain;
using Domain.Repositories;
using Infrastructure.DataBase.Context;
using Infrastructure.Repositories;
namespace Infrastructure.DataBase;

public class UnitOfWork : IUnitOfWork
{
    private readonly ServerDbContext _context;

    private bool _disposed;
    public UnitOfWork(ServerDbContext context)
    {
        _context = context;
        Users = new UserRepository(_context);
        Urls = new UrlRepository(_context);
    }
    public IUserRepository Users { get; }
    public IUrlRepository Urls { get; }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public async void Dispose()
    {
        await Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual async Task Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            await _context.DisposeAsync();
        }
        _disposed = true;
    }
}
