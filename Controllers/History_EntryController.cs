//controller for history_entry procedures

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseProject.Controllers
{
    [Route("api/[controller]")]
    public class History_EntryController : ControllerBase
    {
        public History_EntryController(AppDb db)
        {
            Db = db;
        }

        // GET api/History_Entry/Get/5
        //gets the history entries for a specific customer based on customer ID
        [HttpGet("{customer}")]
        [Route("Get/{customer}")]
        public async Task<IActionResult> GetOne(int customer)
        {
            await Db.Connection.OpenAsync();
            var query = new History_EntryQuery(Db);
            var result = await query.GetOne(customer);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/History_Entry/New
        // creates a new history entry for a customer's reservation
        // enter in new reservation date (mm/dd/yyy), restaurant ID, and customer ID into the body
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> Post([FromBody]History_Entry body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertOne();
            return new OkObjectResult(body);
        }

        public AppDb Db { get; }
    }
}