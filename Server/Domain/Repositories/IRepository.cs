using System.Linq.Expressions;

namespace Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity? Get(int id);
        TEntity? Single(Expression<Func<TEntity, bool>> predicate);
        TEntity? SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity>? Find(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void Remove(TEntity entity);
    }
}
