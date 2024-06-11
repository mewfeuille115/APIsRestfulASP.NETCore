using Movies.Application.Pages;

namespace Movies.Application.Utilities;

public static class IQueryableExtensions
{
	public static IQueryable<T> Page<T>(this IQueryable<T> queryable, PageDto pageDto)
	{
		return queryable
			.Skip(((int)pageDto.Page! - 1) * (int)pageDto.PageSize!)
			.Take((int)pageDto.PageSize);
	}
}
