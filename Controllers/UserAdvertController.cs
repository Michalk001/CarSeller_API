using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAdvertController : ControllerBase
    {

        private readonly ISearchOfferService _searchOfferService;

        public UserAdvertController(ISearchOfferService searchOfferService)
        {
            _searchOfferService = searchOfferService;
        }
        // GET: api/UserAdvert
        [HttpGet]
        public async Task<JsonResult> Get(string page)
        {
            string token = Request.Headers["Authorization"];
            var result = await _searchOfferService.GetByUser(page, token);
            return new JsonResult(result);
        }

        // GET: api/UserAdvert/5


        // POST: api/UserAdvert
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/UserAdvert/5
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
