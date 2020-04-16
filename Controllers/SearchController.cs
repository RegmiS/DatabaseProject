using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseProject.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        public SearchController(AppDb db)
        {
            Db = db;
        }

        // GET api/Search/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new SearchQuery(Db);
            var result = await query.GetAll();
            return new OkObjectResult(result);
        }

        // GET api/Search/Get/5
        [HttpGet("{id}")]
        [Route("Get/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new SearchQuery(Db);
            var result = await query.GetOneSearch(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/Search/New
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> PostOne([FromBody]Search body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertOne();
            return new OkObjectResult(body);
        }

        // PUT api/blog/5
        [HttpPut("{id}")]
        [Route("Update/{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody]Search body)
        {
            await Db.Connection.OpenAsync();
            var query = new SearchQuery(Db);
            var result = await query.GetOneSearch(id);
            if (result is null)
                return new NotFoundResult();
            result.searchID = body.searchID;
            result.customerID = body.customerID;
            await result.UpdateOne();
            return new OkObjectResult(result);
        }

        public AppDb Db { get; }
    }
}