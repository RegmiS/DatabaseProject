//controller for favourites procedures

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DatabaseProject.Controllers
{
    [Route("api/Favourites")]
    public class FavouritesController : ControllerBase
    {
        public FavouritesController(AppDb db)
        {
            Db = db;
        }

        // GET api/Favourites/Get/5
        // gets a specific customer's favourites based on customer ID
        [HttpGet("{id}")]
        [Route("Get/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new FavouritesQuery(Db);
            var result = await query.GetOne(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/Favourites/New
        // creates new favourite to be stored in database
        // enter in customer ID and restaurant ID into database
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> PostOne([FromBody]Favourites body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertOne();
            return new OkObjectResult(body);
        }

        // DELETE api/Favourites/Delete
        // deletes a customer favourite from the database
        // enter in customer ID and restaurant ID to body
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteOne([FromBody]Favourites body)
        {

            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.DeleteOne();
            return new OkObjectResult(body);
        }

        public AppDb Db { get; }
    }
}