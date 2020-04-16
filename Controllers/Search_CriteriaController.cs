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

        // GET api/Search_Criteria/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new Search_CriteriaQuery(Db);
            var result = await query.GetAll();
            return new OkObjectResult(result);
        }

        // GET api/Search_Criteria/Get/5
        [HttpGet("{id}")]
        [Route("Get/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new Search_CriteriaQuery(Db);
            var result = await query.GetOneSearch_Criteria(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/Search_Criteria/New
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> PostOne([FromBody]Search_Criteria body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertOne();
            return new OkObjectResult(body);
        }

        // PUT api/blog/5
        [HttpPut("{id}")]
        [Route("Update/{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody]Search_Criteria body)
        {
            await Db.Connection.OpenAsync();
            var query = new Search_CriteriaQuery(Db);
            var result = await query.GetOneSearch_Criteria(id);
            if (result is null)
                return new NotFoundResult();
            result.searchID = body.searchID;
            result.customerID = body.customerID;
            result.searchCriteria = body.searchCriteria;
            await result.UpdateOne();
            return new OkObjectResult(result);
        }

        public AppDb Db { get; }
    }
}