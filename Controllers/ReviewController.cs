using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DatabaseProject.Controllers
{
    [Route("api/Review")]
    public class ReviewController : ControllerBase
    {
        public ReviewController(AppDb db)
        {
            Db = db;
        }

        // GET api/Review/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new ReviewQuery(Db);
            var result = await query.GetAll();
            return new OkObjectResult(result);
        }

        // GET api/Review/Get/5
        [HttpGet("{id}")]
        [Route("Get/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new ReviewQuery(Db);
            var result = await query.GetOneReview(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/Review/New
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> PostOne([FromBody]Review body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertOne();
            return new OkObjectResult(body);
        }

        // PUT api/blog/5
        [HttpPut("{id}")]
        [Route("Update/{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody]Review body)
        {
            await Db.Connection.OpenAsync();
            var query = new ReviewQuery(Db);
            var result = await query.GetOneReview(id);
            if (result is null)
                return new NotFoundResult();
            result.reviewNumber = body.reviewNumber;
            result.comment = body.comment;
            result.rating = body.rating;
            result.restaurantID = body.restaurantID;
            result.customerID = body.customerID;
            result.date = body.date;
            await result.UpdateOne();
            return new OkObjectResult(result);
        }

        public AppDb Db { get; }
    }
}