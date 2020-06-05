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
    public class EquipmentController : ControllerBase
    {

        private readonly IEquipmentService _equipmentService;

        public EquipmentController(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }
        // GET: api/Equipment
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            var result = await _equipmentService.Get();
            return new JsonResult(result);
        }

        // GET: api/Equipment/5
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(string id)
        {
            var model = await _equipmentService.Get(id);
            return new JsonResult(model);
        }

        // POST: api/Equipment
        [HttpPost]
        public async Task<object> Post([FromBody] EquipmentViewModel model)
        {
            var result = await _equipmentService.Save(model);
            return new JsonResult(result);

        }

        // PUT: api/Equipment/5
        [HttpPut("{id}")]
        public async Task<JsonResult> Put(string id, [FromBody] EquipmentViewModel model)
        {
            var result = await _equipmentService.Update(model);
            return new JsonResult(result);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(string id)
        {
            var result = await _equipmentService.Remove(id);
            return new JsonResult(result);
        }
    }
}
