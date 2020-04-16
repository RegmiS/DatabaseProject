//controller for search procedures

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

        // GET api/Search/Get/5
        // gets specific search information based on search ID
        [HttpGet("{sid}")]
        [Route("Get/{sid}")]
        public async Task<IActionResult> GetOne(int sid)
        {
            await Db.Connection.OpenAsync();
            var query = new SearchQuery(Db);
            var result = await query.GetOneSearch(sid);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/Search/New
        // creates new search and puts it in the database
        // enter in the customer ID into the body
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> PostOne([FromBody]Search body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertOne();
            return new OkObjectResult(body);
        }

        public AppDb Db { get; }
    }
}