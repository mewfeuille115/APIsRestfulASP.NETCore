#nullable disable
using Microsoft.AspNetCore.Mvc;
using Movies.API.Responses;

namespace Movies.API.Helpers;

public class HypermediaLinkService(IUrlHelper urlHelper)
{
	public LinkBase AddSelfLink(string routeName, object routeValues)
	{
		return new LinkBase
		{
			Href = urlHelper.Link(routeName, routeValues),
			Method = "GET",
			Rel = "self",
		};
	}

	public LinkBase AddCreateLink(string routeName, string rel)
	{
		if (string.IsNullOrEmpty(routeName))
		{
			throw new ArgumentNullException(nameof(routeName));
		}
		return new LinkBase
		{
			Href = urlHelper.Link(routeName, new { }),
			Method = "POST",
			Rel = $"create-{rel.ToLower()}",
		};
	}

	public LinkBase AddUpdateLink(string routeName, string rel)
	{
		if (string.IsNullOrEmpty(routeName))
		{
			throw new ArgumentNullException(nameof(routeName));
		}
		return new LinkBase
		{
			Href = urlHelper.Link(routeName, new { }),
			Method = "PUT",
			Rel = $"update-{rel.ToLower()}",
		};
	}

	public LinkBase AddPartialUpdateLink(string routeName, object routeValues, string rel)
	{
		if (string.IsNullOrEmpty(routeName))
		{
			throw new ArgumentNullException(nameof(routeName));
		}
		return new LinkBase
		{
			Href = urlHelper.Link(routeName, routeValues),
			Method = "PATCH",
			Rel = $"partial-update-{rel.ToLower()}",
		};
	}

	public LinkBase AddDeleteLink(string routeName, object routeValues, string rel)
	{
		return new LinkBase
		{
			Href = urlHelper.Link(routeName, routeValues),
			Method = "DELETE",
			Rel = $"delete-{rel.ToLower()}",
		};
	}
}
