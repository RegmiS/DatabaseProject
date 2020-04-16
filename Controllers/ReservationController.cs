using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DatabaseProject.Controllers
{
    [Route("api/Reservation")]
    public class ReservationController : ControllerBase
    {
        public ReservationController(AppDb db)
        {
            Db = db;
        }

        // GET api/Reservation/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new ReservationQuery(Db);
            var result = await query.GetAll();
            return new OkObjectResult(result);
        }

        // GET api/Reservation/Get/5
        [HttpGet("{id}")]
        [Route("Get/{id}")]
        public async Task<IActionResult> GetOneReservation(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new ReservationQuery(Db);
            var result = await query.GetOneReservation(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/Reservation/New
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> PostOne([FromBody]Reservation body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertOne();
            return new OkObjectResult(body);
        }

        // PUT api/blog/5
        [HttpPut("{id}")]
        [Route("Update/{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody]Reservation body)
        {
            await Db.Connection.OpenAsync();
            var query = new ReservationQuery(Db);
            var result = await query.GetOneReservation(id);
            if (result is null)
                return new NotFoundResult();
            result.reservationID = body.reservationID;
            result.crDistance = body.crDistance;
            result.date = body.date;
            result.restaurantID  = body.restaurantID;
            result.customerID = body.customerID;
            await result.UpdateOne();
            return new OkObjectResult(result);
        }

        public AppDb Db { get; }
    }
}