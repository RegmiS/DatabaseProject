using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseProject.Controllers
{
    [Route("api/Food_Property")]
    public class Food_PropertyController : ControllerBase
    {
        public Food_PropertyController(AppDb db)
        {
            Db = db;
        }

        // GET api/Food_Property/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new Food_PropertyQuery(Db);
            var result = await query.GetAll();
            return new OkObjectResult(result);
        }

        // GET api/Food_Property/Get/Pizza
        [HttpGet("{fname}")]
        [Route("Get/{fname}")]
        public async Task<IActionResult> GetOne(string fname)
        {
            await Db.Connection.OpenAsync();
            var query = new Food_PropertyQuery(Db);
            var result = await query.GetOne(fname);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/Food_Property/New
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> Post([FromBody]Food_Property body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertOne();
            return new OkObjectResult(body);
        }

        public AppDb Db { get; }
    }
}