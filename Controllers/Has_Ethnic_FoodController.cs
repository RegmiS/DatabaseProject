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

        // GET api/Has_Ethnic_Food/Get/5
        [HttpGet("{id}")]
        [Route("Get/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new Has_Ethnic_FoodQuery(Db);
            var result = await query.GetOne(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/Has_Ethnic_Food/New
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> Post([FromBody]Has_Ethnic_Food body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertOne();
            return new OkObjectResult(body);
        }

        // DELETE api/Has_Ethnic_Food/Delete
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteOne([FromBody]Has_Ethnic_Food body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.DeleteOne();
            return new OkObjectResult(body);
        }

        public AppDb Db { get; }
    }
}