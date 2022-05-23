using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using WebCommon.Model;
using Microsoft.Identity.Web;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace WebCommon.Service
{
    public class ApiClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ApiClient> _logger;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly IConfiguration _configuration;

        public ApiClient(HttpClient client, ILogger<ApiClient> logger, ITokenAcquisition tokenAcquisition, IConfiguration configuration)
        {
            _client = client;
            _logger = logger;
            _tokenAcquisition = tokenAcquisition;
            _configuration = configuration;
        }

        public async Task<List<T>> Get<T>(string endpoint)
        {
            _logger.LogInformation(String.Format("Fetching data from: {0}", new Uri(_client.BaseAddress, endpoint)));

            var scope = _configuration["CallApi:ScopeForAccessToken"];
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { scope });
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _client.GetAsync(new Uri(_client.BaseAddress, endpoint));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await response.Content.ReadFromJsonAsync<List<T>>();
            }
            var ex = new HttpRequestException("WebApi request error");
            _logger.LogError(ex.Message);
            throw ex;
        }
    }
}
