using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebCommon.Model;

namespace WebCommon.Service
{
    public class DatabaseTablesService
    {
        private readonly ApiClient _apiClient;
        protected readonly ILogger<DatabaseTablesService> _logger;

        public DatabaseTablesService(ApiClient client, ILogger<DatabaseTablesService> logger)
        {
            _apiClient = client;
            _logger = logger;
        }

        public async Task<DatabaseTable[]> GetDatabaseTablesAsync()
        {
            try
            {
                return (await _apiClient.Get<DatabaseTable>("/databasetables")).ToArray();                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
    }
}
