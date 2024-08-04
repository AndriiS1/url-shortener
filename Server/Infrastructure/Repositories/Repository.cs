using System.Linq.Expressions;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories;

public class Repository<TEntity>(DbContext context) : IRepository<TEntity> where TEntity : class
{
    public async Task Add(TEntity entity)
    {
        await context.AddAsync(entity);
    }

    public async Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> predicate)
    {
        return await context.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public IEnumerable<bool> Select(Expression<Func<TEntity, bool>> predicate)
    {
        return context.Set<TEntity>().Select(predicate);
    }

    public async Task<TEntity?> Get(int id)
    {
        return await context.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity?> SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return await context.Set<TEntity>().SingleOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        return await context.Set<TEntity>().ToListAsync();
    }

    public void Remove(TEntity entity)
    {
        context.Remove(entity);
    }
}
