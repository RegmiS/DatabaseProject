using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseProject.Controllers
{
    [Route("api/[controller]")]
    public class Food_PropertyController : ControllerBase
    {
        public Food_PropertyController(AppDb db)
        {
            Db = db;
        }

        // GET api/blog
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new Food_PropertyQuery(Db);
            var result = await query.LatestPostsAsync();
            return new OkObjectResult(result);
        }

        // GET api/blog/5
        [HttpGet("{foodName}")]
        public async Task<IActionResult> GetAll(string foodName)
        {
            await Db.Connection.OpenAsync();
            var query = new Food_PropertyQuery(Db);
            var result = await query.FindAllAsync(foodName);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/blog
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Food_Property body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        // PUT api/blog/5
        [HttpPut("{foodName}")]
        public async Task<IActionResult> PutOne(string foodName, [FromBody]Food_Property body)
        {
            await Db.Connection.OpenAsync();
            var query = new Food_PropertyQuery(Db);
            var result = await query.FindOneAsync(foodName);
            if (result is null)
                return new NotFoundResult();
            result.foodProperty = body.foodProperty;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        // DELETE api/blog/5
        [HttpDelete("{foodName}")]
        public async Task<IActionResult> DeleteOne(string foodName)
        {
            await Db.Connection.OpenAsync();
            var query = new Food_PropertyQuery(Db);
            var result = await query.FindOneAsync(foodName);
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
            var query = new Food_PropertyQuery(Db);
            await query.DeleteAllAsync();
            return new OkResult();
        }

        public AppDb Db { get; }
    }
}