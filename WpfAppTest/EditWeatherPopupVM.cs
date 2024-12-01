using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfAppTest.Commands;
using WpfAppTest.Models;

namespace WpfAppTest
{
	internal class EditWeatherPopupVM: INotifyPropertyChanged
	{
		public WeatherForecast weatherForecast {  get; set; }


		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}

		public EditWeatherPopupVM()
		{
			if (weatherForecast == null)
			{
				weatherForecast = new WeatherForecast();
				weatherForecast.Date = DateTime.UtcNow.ToLocalTime();
			}
		}

		private void SaveWeather(object commandParameter)
		{ 
			
		}

	}
}
