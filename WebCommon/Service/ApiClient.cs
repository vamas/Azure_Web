using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using WebCommon.Model;

namespace WebCommon.Service
{
    public class ApiClient
    {
        protected readonly HttpClient _client;
        protected readonly ILogger<ApiClient> _logger;

        public ApiClient(HttpClient client, ILogger<ApiClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<List<WeatherForecast>> GetWeatherForecast()
        {
            _logger.LogInformation(String.Format("Fetching data from: {0}", _client.BaseAddress + "WeatherForecast"));
            var response = await _client.GetAsync(_client.BaseAddress + "WeatherForecast");            
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await response.Content.ReadFromJsonAsync<List<WeatherForecast>>();
            }
            var ex = new HttpRequestException("WebApi request error");
            _logger.LogError(ex.Message);
            throw ex;
        }


        public async Task<List<DatabaseTable>> GetDatabaseTables()
        {
            _logger.LogInformation(String.Format("Fetching data from: {0}", _client.BaseAddress + "databasetables"));
            var response = await _client.GetAsync(_client.BaseAddress + "databasetables");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await response.Content.ReadFromJsonAsync<List<DatabaseTable>>();
            }
            var ex = new HttpRequestException("WebApi request error");
            _logger.LogError(ex.Message);
            throw ex;
        }


    }
}
