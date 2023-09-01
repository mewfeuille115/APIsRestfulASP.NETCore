namespace WebAPIAutores.DTOs
{
	public class PaginacionDto
	{
		public int Pagina { get; set; } = 1;
		private int _recordsPorPagina = 10;
		private readonly int _cantidadMaximaPorPagina = 50;

		public int RecordsPorPagina
		{
			get
			{
				return _recordsPorPagina;
			}
			set
			{ 
				_recordsPorPagina = (value > _cantidadMaximaPorPagina) ? _cantidadMaximaPorPagina : value;
			}
		}
	}
}
