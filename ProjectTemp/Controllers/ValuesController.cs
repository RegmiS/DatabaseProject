using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace ProjectTemp.Controllers
{
    [Route("api/ValuesController")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/ValuesController/GetValues
        [HttpGet]
        [Route("GetValues")]
        public ActionResult<IEnumerable<string>> GetValues()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/ValuesController/GetValuesById?id=5
        [HttpGet]
        [Route("GetValuesById")]
        public ActionResult<IEnumerable<string>> GetValuesById(int id)
        {
            return new string[] { "value1" };
        }

        // PUT api/ValuesController/InsertEmployee
        [HttpPut]
        [Route("InsertEmployee")]
        public void InsertEmployee([FromBody] JObject emp)
        {
            string empName = (string)emp["empName"];
            string empBdate = (string)emp["empBdate"];
        }

        // PUT api/ValuesController/UpdateEmployee
        [HttpPost]
        [Route("UpdateEmployee")]
        public void UpdateEmployee([FromBody] JObject emp)
        {
            int empId = (int)emp["empId"];
            string empName = (string)emp["empName"];
            string empBdate = (string)emp["empBdate"];
        }


    }
}
