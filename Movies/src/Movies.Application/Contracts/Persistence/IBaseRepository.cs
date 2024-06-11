using Movies.Application.Pages;

namespace Movies.Application.Contracts.Persistence;

public interface IBaseRepository<T, TId> : IDisposable where T : class
{
	Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken);
	Task<IReadOnlyList<T>> ListAllAsync(PageDto page, CancellationToken cancellationToken);
	Task<T?> AddAsync(T entity, CancellationToken cancellationToken);
	T? Update(T entity);
	void Delete(T entity);

	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
