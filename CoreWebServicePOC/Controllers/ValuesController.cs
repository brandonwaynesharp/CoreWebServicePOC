using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreWebServicePOC.core;

namespace CoreWebServicePOC.Controllers
{   
       
    [FormatFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IValuesBusiness _business;

        public ValuesController(IValuesBusiness business)
        {
            _business = business;
        }

        // GET api/values
        [HttpGet("")]
        public async Task<OkObjectResult> Get()
        {
            return Ok(await _business.Get());           
        }

        // GET api/values/5
        [HttpGet("{id}.{format?}")]
        public ActionResult<string> Get(int id)
        {
            return Ok(new string[] { "value1", "value2" });

        }

        // POST api/values
        [HttpPost("")]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
