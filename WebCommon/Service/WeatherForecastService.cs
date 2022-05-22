using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebCommon.Model;

namespace WebCommon.Service
{
    public class WeatherForecastService
    {
        private readonly ApiClient _apiClient;
        private readonly ILogger<WeatherForecastService> _logger;


        public WeatherForecastService(ApiClient apiClient, ILogger<WeatherForecastService> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            try
            {
                return (await _apiClient.GetWeatherForecast()).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
    }
}
