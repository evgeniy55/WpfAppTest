using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPITest.DbContexts;

namespace WebAPITest.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger)
		{
			_logger = logger;
		}

		[HttpGet("GetRandom")]
		public IEnumerable<WeatherForecast> GetRandom()
		{
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
			.ToArray();
		}

		[HttpGet("GetWeatherForecasts")]
		public IEnumerable<WeatherForecast> GetWeatherForecasts()
		{
			List<WeatherForecast> result;
			using (var db = new WeatherForecastDbContext())
			{
				result = db.WeatherForecasts.AsNoTracking().ToList();
			}
			return result;
		}

		[HttpPost("Add")]
		public JsonResult Add(WeatherForecast weather)
		{
			using (var db = new WeatherForecastDbContext())
			{
				if (weather.Id == 0)
				{
					db.WeatherForecasts.Add(weather);
					db.SaveChanges();
				}
			}

			return new JsonResult(weather);
		}


		[HttpPut("Update")]
		public JsonResult Update(WeatherForecast weather)
		{
			using (var db = new WeatherForecastDbContext())
			{
				db.WeatherForecasts.Update(weather);
				db.SaveChanges();
			}

			return new JsonResult(weather);
		}

		[HttpDelete("Delete/{weatherId}")]
		public JsonResult Delete(int weatherId)
		{
			using (var db = new WeatherForecastDbContext())
			{
				var w = db.WeatherForecasts.Find(weatherId);
				db.WeatherForecasts.Remove(w);
				db.SaveChanges();
			}
			return new JsonResult(0);
		}
	}
}
