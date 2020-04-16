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