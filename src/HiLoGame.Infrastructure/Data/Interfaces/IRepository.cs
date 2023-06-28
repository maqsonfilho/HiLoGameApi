namespace HiLoGame.Infrastructure.Data.Interfaces;

public interface IRepository<TEntity> where TEntity : class 
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(Guid id);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> Update(TEntity entity);
    Task Delete(Guid id);
}
