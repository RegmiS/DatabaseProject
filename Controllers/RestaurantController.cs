using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseProject.Controllers
{
    [Route("api/Restaurant")]
    public class RestaurantController : ControllerBase
    {
        public RestaurantController(AppDb db)
        {
            Db = db;
        }

        // GET api/Restaurant/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new RestaurantQuery(Db);
            var result = await query.GetAll();
            return new OkObjectResult(result);
        }

        // GET api/Restaurant/Get/5
        [HttpGet("{id}")]
        [Route("Get/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new RestaurantQuery(Db);
            var result = await query.GetOneRestaurant(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/Restaurant/New
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> PostOne([FromBody]Restaurant body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertOne();
            return new OkObjectResult(body);
        }

        // PUT api/blog/5
        [HttpPut("{id}")]
        [Route("Update/{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody]Restaurant body)
        {
            await Db.Connection.OpenAsync();
            var query = new RestaurantQuery(Db);
            var result = await query.GetOneRestaurant(id);
            if (result is null)
                return new NotFoundResult();
            result.address = body.address;
            result.name = body.name;
            result.priceRange = body.priceRange;
            result.ownerID = body.ownerID;
            await result.UpdateOne();
            return new OkObjectResult(result);
        }

        public AppDb Db { get; }
    }
}