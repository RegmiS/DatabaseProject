using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseProject.Controllers
{
    [Route("api/[controller]")]
    public class Has_Ethnic_FoodController : ControllerBase
    {
        public Has_Ethnic_FoodController(AppDb db)
        {
            Db = db;
        }

        // GET api/blog
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new Has_Ethnic_FoodQuery(Db);
            var result = await query.LatestPostsAsync();
            return new OkObjectResult(result);
        }

        // GET api/blog/5
        [HttpGet("{restaurantID}")]
        public async Task<IActionResult> GetAll(int restaurantID)
        {
            await Db.Connection.OpenAsync();
            var query = new Has_Ethnic_FoodQuery(Db);
            var result = await query.FindAllAsync(restaurantID);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/blog
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Has_Ethnic_Food body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        // PUT api/blog/5
        [HttpPut("{restaurantID}")]
        public async Task<IActionResult> PutOne(int restaurantID, [FromBody]Has_Ethnic_Food body)
        {
            await Db.Connection.OpenAsync();
            var query = new Has_Ethnic_FoodQuery(Db);
            var result = await query.FindOneAsync(restaurantID);
            if (result is null)
                return new NotFoundResult();
            result.foodName = body.foodName;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        // DELETE api/blog/5
        [HttpDelete("{restaurantID}")]
        public async Task<IActionResult> DeleteOne(int restaurantID)
        {
            await Db.Connection.OpenAsync();
            var query = new Has_Ethnic_FoodQuery(Db);
            var result = await query.FindOneAsync(restaurantID);
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
            var query = new Has_Ethnic_FoodQuery(Db);
            await query.DeleteAllAsync();
            return new OkResult();
        }

        public AppDb Db { get; }
    }
}