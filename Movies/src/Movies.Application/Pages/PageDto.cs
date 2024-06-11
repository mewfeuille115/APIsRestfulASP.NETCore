namespace Movies.Application.Pages;

public record PageDto()
{
	private int _page = 1;
	private int _pageSize = 10;
	private readonly int _maxSize = 50;

	public int? Page
	{
		get => _page;
		set
		{
			// Checks if value is null, if so, assign the default page.
			if (value is null || value.Equals(0))
				value = _page;

			_page = (int)value;
		}
	}

	public int? PageSize
	{
		get => _pageSize;

		set
		{
			// Checks if value is null, if so, assign the default size.
			if (value is null || value.Equals(0))
				value = _pageSize;

			_pageSize = (value >= _maxSize) ? _maxSize : (int)value;
		}
	}
}
