using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseProject.Controllers
{
    [Route("api/[controller]")]
    public class Ethnic_FoodController : ControllerBase
    {
        public Ethnic_FoodController(AppDb db)
        {
            Db = db;
        }

        // GET api/blog
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new Ethnic_FoodQuery(Db);
            var result = await query.LatestPostsAsync();
            return new OkObjectResult(result);
        }

        // GET api/blog/5
        [HttpGet("{name}")]
        public async Task<IActionResult> GetOne(string name)
        {
            await Db.Connection.OpenAsync();
            var query = new Ethnic_FoodQuery(Db);
            var result = await query.FindOneAsync(name);
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
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        // PUT api/blog/5
        [HttpPut("{name}")]
        public async Task<IActionResult> PutOne(string name, [FromBody]Ethnic_Food body)
        {
            await Db.Connection.OpenAsync();
            var query = new Ethnic_FoodQuery(Db);
            var result = await query.FindOneAsync(name);
            if (result is null)
                return new NotFoundResult();
            result.ethnicity = body.ethnicity;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        // DELETE api/blog/5
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteOne(string name)
        {
            await Db.Connection.OpenAsync();
            var query = new Ethnic_FoodQuery(Db);
            var result = await query.FindOneAsync(name);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkResult();
        }

        // DELETE api/blog
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await Db.Connection.OpenAsync();
            var query = new Ethnic_FoodQuery(Db);
            await query.DeleteAllAsync();
            return new OkResult();
        }

        public AppDb Db { get; }
    }
}