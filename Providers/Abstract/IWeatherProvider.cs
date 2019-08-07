using System.Collections.Generic;
using VueExample.Models;

namespace VueExample.Providers
{
    public interface IWeatherProvider
    {
        List<WeatherForecast> GetForecasts();
    }
}
