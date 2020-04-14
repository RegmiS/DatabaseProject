using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DatabaseProject.Controllers
{
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        public ReviewController(AppDb db)
        {
            Db = db;
        }

        // GET api/blog
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new ReviewQuery(Db);
            var result = await query.LatestPostsAsync();
            return new OkObjectResult(result);
        }

        // GET api/blog/5
        [HttpGet("{reviewNumber}")]
        public async Task<IActionResult> GetOne(int reviewNumber)
        {
            await Db.Connection.OpenAsync();
            var query = new ReviewQuery(Db);
            var result = await query.FindOneAsync(reviewNumber);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/blog
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Review body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        // PUT api/blog/5
        [HttpPut("{reviewNumber}")]
        public async Task<IActionResult> PutOne(int reviewNumber, [FromBody]Review body)
        {
            await Db.Connection.OpenAsync();
            var query = new ReviewQuery(Db);
            var result = await query.FindOneAsync(reviewNumber);
            if (result is null)
                return new NotFoundResult();
            result.comment = body.comment;
            result.rating = body.rating;
            result.restaurantID = body.restaurantID;
            result.customerID = body.customerID;
            result.date = body.date;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        // DELETE api/blog/5
        [HttpDelete("{reviewNumber}")]
        public async Task<IActionResult> DeleteOne(int reviewNumber)
        {
            await Db.Connection.OpenAsync();
            var query = new ReviewQuery(Db);
            var result = await query.FindOneAsync(reviewNumber);
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
            var query = new ReviewQuery(Db);
            await query.DeleteAllAsync();
            return new OkResult();
        }

        public AppDb Db { get; }
    }
}