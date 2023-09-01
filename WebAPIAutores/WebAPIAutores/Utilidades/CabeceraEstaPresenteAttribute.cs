using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace WebAPIAutores.Utilidades
{
	public class CabeceraEstaPresenteAttribute : Attribute, IActionConstraint
	{
		private readonly string _cabecera;
		private readonly string _valor;

		public CabeceraEstaPresenteAttribute(string cabecera, string valor)
		{
			_cabecera = cabecera;
			_valor = valor;
		}

		public int Order => 0;

		public bool Accept(ActionConstraintContext context)
		{
			var cabeceras = context.RouteContext.HttpContext.Request.Headers;

			if (!cabeceras.ContainsKey(_cabecera))
				return false;

			return string.Equals(cabeceras[_cabecera], _valor, StringComparison.OrdinalIgnoreCase);
		}
	}
}
