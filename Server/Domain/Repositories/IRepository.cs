using System.Linq.Expressions;
namespace Domain.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> Get(int id);
    Task<TEntity?> SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetAll();
    Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> predicate);
    IEnumerable<bool> Select(Expression<Func<TEntity, bool>> predicate);
    Task Add(TEntity entity);
    void Remove(TEntity entity);
}
