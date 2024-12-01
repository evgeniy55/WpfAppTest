using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfAppTest.Commands;
using WpfAppTest.Models;

namespace WpfAppTest
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		public ObservableCollection<WeatherForecast> WeatherForecasts { get; set; }

		public string TestString { get; set; }

		public ICommand GetWeathersCommand { get; }
		public ICommand AddWeatherCommand { get; }
		public ICommand UpdateWeatherCommand { get; }
		public ICommand DeleteWeatherCommand { get; }

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}

		public MainWindowViewModel()
		{

			GetWeathersCommand = new RelayCommand(GetWeatherForecasts);
			AddWeatherCommand = new RelayCommand(AddWeatherDialog);
			UpdateWeatherCommand = new RelayCommand(UpdateWeatherDialog);
			DeleteWeatherCommand = new RelayCommand(DeleteWeatherDialog);

			TestString = "teststring";
		}


		private async void GetWeatherRandom(object commandParameter)
		{
			using (var client = new HttpClient())
			{
				var result = await client.GetFromJsonAsync("http://localhost:5178/WeatherForecast/GetRandom", typeof(IEnumerable<WeatherForecast>));
				if (result is IEnumerable)
				{
					var resultTyped = result as IEnumerable<WeatherForecast>;
					WeatherForecasts = new ObservableCollection<WeatherForecast>(resultTyped); 
					OnPropertyChanged(nameof(WeatherForecasts));
				}
			}
		}

		private async void GetWeatherForecasts(object commandParameter)
		{
			LoadWeatherForecasts();
		}


		private async void AddWeatherDialog(object commandParameter)
		{
			var dialog = new EditWeatherPopup();
			if (dialog.ShowDialog() == true){
				if (dialog.DataContext == null)
					return;
				
				var weather = ((EditWeatherPopupVM)dialog.DataContext).weatherForecast;
				var result = await WeatherForecast.AddAsync(weather);
				if (result != null) {
					LoadWeatherForecasts();
				}
			}
		}

		private async void UpdateWeatherDialog(object commandParameter)
		{
			if (commandParameter == null)
			{
				return;
			}
			var dialog = new EditWeatherPopup();
			var vm = new EditWeatherPopupVM();

			var updatingWeather = (WeatherForecast)commandParameter;
			vm.weatherForecast = new WeatherForecast()
			{
				Id = updatingWeather.Id,
				Date = updatingWeather.Date,
				TemperatureC = updatingWeather.TemperatureC,
				Summary = updatingWeather.Summary
			};

			dialog.DataContext = vm;

			if (dialog.ShowDialog() == true)
			{

				var weather = ((EditWeatherPopupVM)dialog.DataContext).weatherForecast;
				var result = await WeatherForecast.UpdateAsync(weather);
				if (result != null)
				{
					LoadWeatherForecasts();
				}
			}
		}


		private async void DeleteWeatherDialog(object commandParameter)
		{
			var dialog = MessageBox.Show("Удалить выбранную запись?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
			
			if (dialog == MessageBoxResult.Yes)
			{
				if (commandParameter != null) 
				{
					var selectedWeather = (WeatherForecast)commandParameter;

					var result = await WeatherForecast.DeleteAsync(selectedWeather.Id);
					if (result == 0) {
						LoadWeatherForecasts();
					}
				}
			}
		}

		private async void LoadWeatherForecasts()
		{
			var forecasts = await WeatherForecast.GetWeatherForecastsAsync();
			if (forecasts != null)
			{
				WeatherForecasts = new ObservableCollection<WeatherForecast>(forecasts);
				OnPropertyChanged(nameof(WeatherForecasts));
			}
		}
	}
}
