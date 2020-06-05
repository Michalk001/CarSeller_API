using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Service;
using API.ServiceInterfaces;
using API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarOfferController : ControllerBase
    {

        private readonly ICarOfferService _carOfferService;
        public CarOfferController(ICarOfferService carOfferService)
        {
            _carOfferService = carOfferService;
        }
        // GET: api/Equipment
        [HttpGet]
        public async Task<JsonResult> Get()
        {
           
            return new JsonResult(null);
        }

        // GET: api/Equipment/5
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(string id)
        {
            var model = await _carOfferService.Get(id);
            return new JsonResult(model);
        }

        // POST: api/Equipment
        [HttpPost]
        [Authorize]
        public async Task Post([FromBody] CarOfferViewModel _model)
        {
            string token = Request.Headers["Authorization"];
            await _carOfferService.Save(_model, token);
            this.HttpContext.Response.StatusCode = 201;

        }

        // PUT: api/Equipment/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] CarOfferViewModel model)
        {
            string token = Request.Headers["Authorization"];
            try
            {
                await _carOfferService.Update(model,token);


                this.HttpContext.Response.StatusCode = 204;
                return;
            }
            catch (Exception ex)
            {
                this.HttpContext.Response.StatusCode = 404;
                throw ex;
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<JsonResult> Delete(string id)
        {
            string token = Request.Headers["Authorization"];
            var result = await _carOfferService.Remove(id, token);
            return new JsonResult(result);
        }
    }
}
