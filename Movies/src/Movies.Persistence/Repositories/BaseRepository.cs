using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Movies.Application.Contracts.Persistence;
using Movies.Application.Pages;
using Movies.Application.Utilities;
using Movies.Domain.Common;

namespace Movies.Persistence.Repositories;

public class BaseRepository<T, TId> : IBaseRepository<T, TId> where T : AuditableEntity
{
	protected readonly ApplicationDbContext _dbContext;
	protected DbSet<T> Entities => _dbContext.Set<T>();
	private bool disposed = false;

	public BaseRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken)
		=> await _dbContext.Set<T>().FindAsync([id], cancellationToken);

	public async Task<IReadOnlyList<T>> ListAllAsync(PageDto page, CancellationToken cancellationToken)
	=> await _dbContext.Set<T>()
		.Page(page)
		.ToListAsync(cancellationToken);

	public async Task<T?> AddAsync(T entity, string userId, CancellationToken cancellationToken)
	{
		entity.CreatedBy = userId;
		entity.CreatedDate = DateTime.UtcNow;
		EntityEntry<T> insertedValue = await _dbContext.Set<T>().AddAsync(entity, cancellationToken);

		return insertedValue?.Entity;
	}

	public T? Update(T entity, string userId)
	{
		entity.LastModifiedBy = userId;
		entity.LastModifiedDate = DateTime.UtcNow;

		return _dbContext.Set<T>().Update(entity)?.Entity;
	}

	public void Delete(T entity)
	{
		_dbContext.Set<T>().Remove(entity);
	}

	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
		=> await _dbContext.SaveChangesAsync(cancellationToken);

	protected virtual void Dispose(bool disposing)
	{
		if (!this.disposed && disposing) _dbContext.Dispose();

		this.disposed = true;
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
