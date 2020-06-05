using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.ServiceInterfaces;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarProducentController : ControllerBase
    {
        // GET: api/CarProducent
        private readonly ICarProducentService _carProducentService;


        public CarProducentController(ICarProducentService carProducentService)
        {
            _carProducentService = carProducentService;
        }
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            var models = await _carProducentService.Get();
            
            return new JsonResult( models);
        }

        // GET: api/CarProducent/5
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(string id)
        {
            var model = await _carProducentService.Get(id);
            return new JsonResult(model);
        }

        // POST: api/CarProducent
        [HttpPost]
        
        public async Task <JsonResult> Post([FromBody] CarProducentViewModel model)
        {
           var result =  await _carProducentService.Save(model);
           return new JsonResult(result);
        }

        // PUT: api/CarProducent/5
        [HttpPut("{id}")]
        public async Task<JsonResult> Put(string id, [FromBody] CarProducentViewModel model)
        {
      
                var result =  await _carProducentService.Update(model);
                return new JsonResult(result);

            }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(string id)
        {
            var result = await _carProducentService.Remove(id);
            return new JsonResult(result);
        }
    }
}
