using Movies.Application.Pages;
using Movies.Domain.Common;

namespace Movies.Application.Contracts.Persistence;

public interface IBaseRepository<T, TId> : IDisposable where T : AuditableEntity
{
	Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken);
	Task<IReadOnlyList<T>> ListAllAsync(PageDto page, CancellationToken cancellationToken);
	Task<T?> AddAsync(T entity, string userId, CancellationToken cancellationToken);
	T? Update(T entity, string userId);
	void Delete(T entity);

	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
