using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPITest
{
	public class WeatherForecast
	{
		[Key]
		public int Id { get; set; }
		public DateTime Date { get; set; }

		public int TemperatureC { get; set; }

		[NotMapped]
		public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

		public string? Summary { get; set; }
	}
}
