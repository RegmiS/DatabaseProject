//controller for has_ethnic_food procedures

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
        //gets all the foods a specific restaurant has based on restaurant ID
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
        // add a new food for a restaurant
        // enter in restaurant ID and food name into the body
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
        // deletes a food from a restaurant
        // enter in restaurant ID and food to delete into the body
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