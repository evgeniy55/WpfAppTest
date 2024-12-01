using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPITest.DbContexts
{
	public class WeatherForecastDbContext: DbContext
	{

		
		public DbSet<WeatherForecast> WeatherForecasts { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Data Source=weatherApp.db");
			
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// использование Fluent API
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<WeatherForecast>().ToTable("WeatherForecast");
		}
	}
}
