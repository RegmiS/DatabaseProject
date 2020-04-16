//controller for payment procedures

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseProject.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        public PaymentController(AppDb db)
        {
            Db = db;
        }

        // GET api/Payment/Get/5
        // get specific payment information for a reservation based on payment ID
        [HttpGet("{id}")]
        [Route("Get/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new PaymentQuery(Db);
            var result = await query.GetOne(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/Payment/New
        // creates new payment information for a reservation
        // enter in new reservation ID, payment method, and payment amount into database
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> Post([FromBody]Payment body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertOne();
            return new OkObjectResult(body);
        }

        public AppDb Db { get; }
    }
}