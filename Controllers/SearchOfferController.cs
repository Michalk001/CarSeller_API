using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchOfferController : ControllerBase
    {
        private readonly ISearchOfferService searchOfferService;

        public SearchOfferController(ISearchOfferService searchOfferService)
        {
            this.searchOfferService = searchOfferService;
        }
        [HttpGet]
        public async Task<JsonResult> Get(string page, string producer, string model,string fuel)
        {
            
            
            var result = await searchOfferService.Get(page, producer, model, fuel);
         

            return new JsonResult(result);
        }

        // POST: api/test
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/test/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
