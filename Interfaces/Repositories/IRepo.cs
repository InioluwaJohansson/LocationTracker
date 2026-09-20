using System.Linq.Expressions;

namespace LocationTracker.Interfaces.Repositories;
public interface IRepo<T> 
{
    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task<T> Get(Expression<Func<T, bool>> expression);
    Task<IList<T>> GetAll();
    Task<bool> Delete(T entity);
    Task<IList<T>> GetByExpression(Expression<Func<T, bool>> expression);
}