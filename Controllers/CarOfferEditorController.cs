using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.ServiceInterfaces;
using API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarOfferEditorController : ControllerBase
    {


        private readonly ICarOfferService _carOfferService;
        public CarOfferEditorController(ICarOfferService carOfferService)
        {
            _carOfferService = carOfferService;
        }
        // GET: api/CarOfferEditor
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/CarOfferEditor/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<JsonResult> Get(string id)
        {
            var model = await _carOfferService.GetRaw(id);
            return new JsonResult(model);
        }

        // POST: api/CarOfferEditor
        [HttpPost]
        [Authorize]
        public async Task<object> Post([FromBody] CarOfferViewModel _model)
        {
            string token = Request.Headers["Authorization"];
            return await _carOfferService.Save(_model, token);

        }

        // PUT: api/CarOfferEditor/5
        [HttpPut("{id}")]
        public async Task<object> Put(int id, [FromBody] CarOfferViewModel _model)
        {

            string token = Request.Headers["Authorization"];
            return await _carOfferService.Update(_model, token);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
