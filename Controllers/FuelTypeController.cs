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
    public class FuelTypeController : ControllerBase
    {

        private readonly IFuelTypeService _fuelTypeService;

        public FuelTypeController(IFuelTypeService fuelTypeService)
        {
            _fuelTypeService = fuelTypeService;
        }
        // GET: api/FuelType
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            var models = await _fuelTypeService.Get();
            return new JsonResult(models);
        }

        // GET: api/FuelType/5
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(string id)
        {
            var model = await _fuelTypeService.Get(id);
            return new JsonResult(model);
        }

        // POST: api/FuelType
        [HttpPost]
        public async Task<object> Post([FromBody] FuelTypeViewModel model)
        {
            var result = await _fuelTypeService.Save(model);
            return new JsonResult(result);
        }

        // PUT: api/FuelType/5
        [HttpPut("{id}")]
        public async Task<JsonResult> Put(string id, [FromBody] FuelTypeViewModel model)
        {

            var result = await _fuelTypeService.Update(model);
            return new JsonResult(result);

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(string id)
        {
            var result = await _fuelTypeService.Remove(id);
            return new JsonResult(result);
        }
    }
}
