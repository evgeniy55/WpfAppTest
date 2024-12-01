using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections;

namespace WpfAppTest.Models
{
	public class WeatherForecast
	{
		public int Id { get; set; }

		public DateTime Date { get; set; }

		public int TemperatureC { get; set; }

		public int TemperatureF { get; set; }

		public string? Summary { get; set; }


		public static async Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync()
		{
			IEnumerable <WeatherForecast> weatherForecasts = null;
			using (var client = new HttpClient())
			{
				var result = await client.GetFromJsonAsync("http://localhost:5178/WeatherForecast/GetWeatherForecasts", typeof(IEnumerable<WeatherForecast>));
				if (result is IEnumerable)
				{
					weatherForecasts = result as IEnumerable<WeatherForecast>;
				}
			}
			return weatherForecasts;
		}

		public static async Task<WeatherForecast> AddAsync(WeatherForecast weatherForecast)
		{
			
			using (var client = new HttpClient())
			{
				var response = await client.PostAsync("http://localhost:5178/WeatherForecast/Add", JsonContent.Create(weatherForecast));
				var resultweather = await response.Content.ReadFromJsonAsync<WeatherForecast>();
				return resultweather;
			}
		}

		public static async Task<WeatherForecast> UpdateAsync(WeatherForecast weatherForecast)
		{
			using (var client = new HttpClient())
			{
				var result = await client.PutAsync("http://localhost:5178/WeatherForecast/Update", JsonContent.Create(weatherForecast));
				var updatedWeather = await result.Content.ReadFromJsonAsync<WeatherForecast>();
				return updatedWeather;
			}
		}

		public static async Task<int> DeleteAsync(int weatherId)
		{
			using (var client = new HttpClient())
			{
				var result = await client.DeleteAsync("http://localhost:5178/WeatherForecast/Delete/" + weatherId );
				var deleteResult = await result.Content.ReadFromJsonAsync<int>();
				return deleteResult;
			}
		}
	}
}
