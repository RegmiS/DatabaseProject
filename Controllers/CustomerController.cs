//controller for customer procedures

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseProject.Controllers
{
    [Route("api/Customer")]
    public class CustomerController : ControllerBase
    {
        public CustomerController(AppDb db)
        {
            Db = db;
        }

        // GET api/Customer/GetAll
        // gets all customers in the database
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new CustomerQuery(Db);
            var result = await query.GetAll();
            return new OkObjectResult(result);
        }

        // GET api/Customer/Get/5
        //gets a specific customer based on customer ID
        [HttpGet("{id}")]
        [Route("Get/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new CustomerQuery(Db);
            var result = await query.GetOneCustomer(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/Customer/New
        // puts new customer into database
        // enter in first name, last name, and address into body
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> PostOne([FromBody]Customer body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertOne();
            return new OkObjectResult(body);
        }

        // PUT api/Customer/Update/5
        // updates specific customer information
        // enter in new first name, last name, and address into body
        [HttpPut("{id}")]
        [Route("Update/{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody]Customer body)
        {
            await Db.Connection.OpenAsync();
            var query = new CustomerQuery(Db);
            var result = await query.GetOneCustomer(id);
            if (result is null)
                return new NotFoundResult();
            result.firstName = body.firstName;
            result.lastName = body.lastName;
            result.address = body.address;
            await result.UpdateOne();
            return new OkObjectResult(result);
        }

        public AppDb Db { get; }
    }
}