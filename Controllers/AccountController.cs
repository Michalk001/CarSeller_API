using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.ServiceInterfaces;
using API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _account;
        private readonly UserManager<User> _userManager;
        public AccountController(IAccountService account, UserManager<User> userManager)
        {
            _account = account;
            _userManager = userManager;
        }

    

        [HttpPost]
        public async Task<JsonResult> Login([FromBody] LoginUserViewModel model)
        {
            var result = await _account.Login(model);
            return new JsonResult(result);
           
        }
        [HttpPost]
        public async Task<JsonResult> Register([FromBody] RegisterUserViewModel model)
        {
            var result = await _account.Register(model);
            return new JsonResult(result);
        }
        [Authorize]
        [HttpPut]
        public async Task<JsonResult> ChangePasword([FromBody] UserChangeDataViewModel model)
        {
            string token = Request.Headers["Authorization"];
            var result = await _account.ChangePassword(token, model.PasswordOld,model.PasswordNew);
            return new JsonResult(result);
        }

        [Authorize]
        [HttpPut]
        public async Task<JsonResult> ChangeData([FromBody] UserChangeDataViewModel model)
        {
            string token = Request.Headers["Authorization"];
            var result = await _account.ChangeData(token, model);
            return new JsonResult(result);
        }

    }
}