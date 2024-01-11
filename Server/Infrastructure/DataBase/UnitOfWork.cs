using Domain;
using Domain.Repositories;
using Infrastructure.DataBase.Context;
using Infrastructure.Repositories;

namespace Infrastructure.DataBase
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ServerDbContext _context;
        public IUserRepository Users { get; private set; }

        private bool disposed = false;
        public UnitOfWork(ServerDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                _context.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
