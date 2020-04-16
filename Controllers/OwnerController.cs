//controller for owner procedures

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseProject.Controllers
{
    [Route("api/[controller]")]
    public class OwnerController : ControllerBase
    {
        public OwnerController(AppDb db)
        {
            Db = db;
        }

        // GET api/Owner/GetAll
        // gets all owners in the database
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new OwnerQuery(Db);
            var result = await query.GetAll();
            return new OkObjectResult(result);
        }

        // GET api/Owner/Get/5
        // gets specific owner information based on owner ID
        [HttpGet("{id}")]
        [Route("Get/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new OwnerQuery(Db);
            var result = await query.GetOne(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/Owner/New
        // creates a new owner for the database
        // enter in owner first name and last name into the body
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> PostOne([FromBody]Owner body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertOne();
            return new OkObjectResult(body);
        }

        // PUT api/Owner/Update/5
        // update an existing owner in the database based on owner ID
        // enter in new first name and last name into the body
        [HttpPut("{id}")]
        [Route("Update/{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody]Owner body)
        {
            await Db.Connection.OpenAsync();
            var query = new OwnerQuery(Db);
            var result = await query.GetOne(id);
            if (result is null)
                return new NotFoundResult();
            result.firstName = body.firstName;
            result.lastName = body.lastName;
            await result.UpdateOne();
            return new OkObjectResult(result);
        }

        public AppDb Db { get; }
    }
}