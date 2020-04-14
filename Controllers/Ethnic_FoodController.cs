using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseProject.Controllers
{
    [Route("api/Ethnic_Food")]
    public class Ethnic_FoodController : ControllerBase
    {
        public Ethnic_FoodController(AppDb db)
        {
            Db = db;
        }

        // GET api/Ethnic_Food/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new Ethnic_FoodQuery(Db);
            var result = await query.GetAll();
            return new OkObjectResult(result);
        }

        // GET api/Ethnic_Food/Get/5
        [HttpGet("{name}")]
        [Route("Get/{name}")]
        public async Task<IActionResult> GetOne(string name)
        {
            await Db.Connection.OpenAsync();
            var query = new Ethnic_FoodQuery(Db);
            var result = await query.GetOne(name);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/blog
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Ethnic_Food body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertOne();
            return new OkObjectResult(body);
        }

        // PUT api/blog/5
        [HttpPut("{name}")]
        [Route("Update/{name}")]
        public async Task<IActionResult> PutOne(string name, [FromBody]Ethnic_Food body)
        {
            await Db.Connection.OpenAsync();
            var query = new Ethnic_FoodQuery(Db);
            var result = await query.GetOne(name);
            if (result is null)
                return new NotFoundResult();
            result.ethnicity = body.ethnicity;
            await result.UpdateOne();
            return new OkObjectResult(result);
        }

        public AppDb Db { get; }
    }
}