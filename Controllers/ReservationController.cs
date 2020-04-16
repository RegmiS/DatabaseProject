//controller for reservation procedures

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

        // GET api/Reservation/GetAll/5
        // gets all reservations for a specified restaurant based on restaurant ID
        [HttpGet("{rid}")]
        [Route("GetAll/{rid}")]
        public async Task<IActionResult> GetAll(int rid)
        {
            await Db.Connection.OpenAsync();
            var query = new ReservationQuery(Db);
            var result = await query.GetAll(rid);
            return new OkObjectResult(result);
        }

        // GET api/Reservation/Get/5
        // gets a specific reservation based on reservation ID
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
        // create new reservation
        // enter in date, restaurant ID, and customer ID into the body
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> PostOne([FromBody]Reservation body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertOne();
            return new OkObjectResult(body);
        }

        public AppDb Db { get; }
    }
}