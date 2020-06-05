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
    public class CarModelController : ControllerBase
    {
        // GET: api/CarModel

        private readonly ICarModelService _carModelService;


        public CarModelController(ICarModelService carModelService)
        {
            _carModelService = carModelService;
        }

        [HttpGet]
        public async Task<JsonResult> Get()
        {
            var models = await _carModelService.Get();
            return new JsonResult(models);
        }

        // GET: api/CarModel/5
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(string id)
        {
            var model = await _carModelService.Get(id);
            return new JsonResult(model);
        }

        // POST: api/CarModel
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] CarModelViewModel model)
        {
            var result =  await _carModelService.Save(model);
            return new JsonResult(result);

        }

        // PUT: api/CarModel/5
        [HttpPut("{id}")]
        public async Task<JsonResult> Put(string id, [FromBody] CarModelViewModel model)
        {

            var result = await _carModelService.Update(model);
            return new JsonResult(result);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{idProducer}/{id}")]
        public async Task<JsonResult> Delete(string id,string idProducer)
        {
            var result = await _carModelService.Remove(id, idProducer);
            return new JsonResult(result);
        }
    }
}
