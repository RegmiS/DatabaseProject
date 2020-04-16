//controller for search_criteria procedures

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseProject.Controllers
{
    [Route("api/Search_Criteria")]
    public class Search_CriteriaController : ControllerBase
    {
        public Search_CriteriaController(AppDb db)
        {
            Db = db;
        }

        // GET api/Search_Criteria/Get/5
        // gets a specific search and its criteria based on search ID
        [HttpGet("{sid}")]
        [Route("Get/{sid}")]
        public async Task<IActionResult> GetOne(int sid)
        {
            await Db.Connection.OpenAsync();
            var query = new Search_CriteriaQuery(Db);
            var result = await query.GetOne(sid);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/Search_Criteria/New
        // creates new search criteria and puts it in the database
        // enter in search ID, customer ID, and search criteria into the body (ensure that searchID + customerID exist in the search table)
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> PostOne([FromBody]Search_Criteria body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertOne();
            return new OkObjectResult(body);
        }

        public AppDb Db { get; }
    }
}