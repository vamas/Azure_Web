using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebCommon.Model;

namespace WebAPI.Data
{
    public class Repository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<Repository> _logger;

        public Repository(ILogger<Repository> logger, IConfiguration config)
        {
            _config = config;
            _logger = logger;
        }

        public IEnumerable<DatabaseTable> GetDatabaseTables()
        {
            try
            {
                using (IDbConnection cn = new SqlConnection(_config.GetConnectionString("Database")))
                {                   
                    
                    string sql = $@"SELECT name, object_id, create_date FROM sys.tables";
                    var rows = cn.Query<DatabaseTable>(sql).AsQueryable();
                    return rows;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database query execution error");
                throw new Exception(ex.Message);
            }
        }
    }
}
