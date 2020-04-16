//controller for review procedures

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

        // GET api/Review/GetAll/5
        // gets all reviews for a specified restaurant based on restaurant ID
        [HttpGet("{rid}")]
        [Route("GetAll/{rid}")]
        public async Task<IActionResult> GetAll(int rid)
        {
            await Db.Connection.OpenAsync();
            var query = new ReviewQuery(Db);
            var result = await query.GetAll(rid);
            return new OkObjectResult(result);
        }

        // GET api/Review/Get/5
        // gets a specific review information based on review ID
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
        // creates new review for a restaurant
        // enter in reviwe comment, rating (1 - 5), restaurant ID, customer ID, and date (mm/dd/yyyy)
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> PostOne([FromBody]Review body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertOne();
            return new OkObjectResult(body);
        }

        public AppDb Db { get; }
    }
}