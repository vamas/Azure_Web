using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Data;
using WebCommon.Model;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

namespace WebAPI.Controllers
{
    [Authorize]
    [RequiredScope("databasetables.read")]
    [Route("[controller]")]
    [ApiController]
    public class DatabaseTablesController : ControllerBase
    {
        private Repository _repo;
        private readonly ILogger<DatabaseTablesController> _logger;

        public DatabaseTablesController(ILogger<DatabaseTablesController> logger, Repository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<DatabaseTable> Get()
        {
            _logger.LogInformation("DatabaseTable request handler invoked.");
            return _repo.GetDatabaseTables().ToArray();
        }
    }
}
