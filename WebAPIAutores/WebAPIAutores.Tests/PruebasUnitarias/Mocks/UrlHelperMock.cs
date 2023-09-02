﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Diagnostics.CodeAnalysis;

namespace WebAPIAutores.Tests.PruebasUnitarias.Mocks
{
	public class UrlHelperMock : IUrlHelper
	{
		public ActionContext ActionContext => throw new NotImplementedException();

		public string? Action(UrlActionContext actionContext)
		{
			throw new NotImplementedException();
		}

		[return: NotNullIfNotNull("contentPath")]
		public string? Content(string? contentPath)
		{
			throw new NotImplementedException();
		}

		public bool IsLocalUrl([NotNullWhen(true)] string? url)
		{
			throw new NotImplementedException();
		}

		public string? Link(string? routeName, object? values)
		{
			return "";
		}

		public string? RouteUrl(UrlRouteContext routeContext)
		{
			throw new NotImplementedException();
		}
	}
}
