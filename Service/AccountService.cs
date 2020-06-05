using API.Models;
using API.ServiceInterfaces;
using API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace API.Service
{
    public class AccountService : IAccountService
    {
        private readonly DBContext _context;
        private readonly IOptions<Settings> _settings;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public AccountService(DBContext context,IOptions<Settings> settings, UserManager<User> userManager,
            SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _settings = settings;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<object> Login(LoginUserViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.Include(x=>x.UserRoles).ThenInclude(x => x.Role).SingleOrDefault(r => r.UserName == model.UserName);
                
                return new
                {
                    succeeded = result.Succeeded,
                    Token = GenerateJwtToken(model.UserName, appUser),
                    appUser.FirstName,
                    appUser.SecondName,
                    appUser.PhoneNumber
                };
            }
            return result;
            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }
        public async Task<object> ChangeData(string token, UserChangeDataViewModel model)
        {
            var userName = JwtDecode.User(token);
            var user = await _userManager.FindByNameAsync(userName);
            if (!string.IsNullOrEmpty(model.City))
                user.City = model.City;
            if (!string.IsNullOrEmpty(model.Country))
                user.Country = model.Country;
            if (!string.IsNullOrEmpty(model.PhoneNumber))
                user.PhoneNumber = model.PhoneNumber;
            if (!string.IsNullOrEmpty(model.PostCode))
                user.PostCode = model.PostCode;

            if (!string.IsNullOrEmpty(model.Email))
            {
               var userByEmail = await _userManager.FindByEmailAsync(model.Email);
                if(userByEmail != null)
                    if(userByEmail.UserName == user.UserName)
                        user.Email = model.Email;
                    else
                    {
                        return new
                        {
                            Succeeded = false,
                            Errors = new[]
                            {
                                new
                                {
                                    Code = "BusyEmail",
                                    Description = "This Email is busy by other user"
                                }
                            }
                        };
                    }

            }
           


            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new
                {
                    Token = GenerateJwtToken(user.UserName, user),
                    user.FirstName,
                    user.SecondName,
                    user.PhoneNumber
                };
            }
            return result;
        }

        public async Task<object> ChangePassword(string token, string passwordOld,string passwordNew)
        {
            var userName = JwtDecode.User(token);
            var user = await _userManager.FindByNameAsync(userName);
            var result = await _userManager.ChangePasswordAsync(user, passwordOld, passwordNew);

            if(result.Succeeded)
            {
                return new
                {
                    result.Succeeded,
                    Token = GenerateJwtToken(user.UserName, user),
                    user.FirstName,
                    user.SecondName,
                    user.PhoneNumber
                };
            }

            return result;
        }

        public async Task<object> Register([FromBody] RegisterUserViewModel model)
        {
            try
            {

                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    PhoneNumber = model.PhoneNumber,

                };
                if (model.TypeSeller == "1")
                {
                    user.BusinessProfile = true;
                    user.FirstName = model.CompanyName;
                }
                else
                    user.BusinessProfile = false;
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    var checkRole = await _roleManager.RoleExistsAsync("User");
                    user = await _userManager.FindByNameAsync(user.UserName);
                    if (user != null)
                    {
                        if (!checkRole)
                            await _roleManager.CreateAsync(new Role("User"));
                        //   await _userManager.AddToRoleAsync(user, "User");
                        var Role = await _roleManager.FindByNameAsync("User");
                        var userRoles = (user.UserRoles);
                        userRoles.Add(new UserRole()
                        {
                            Role = Role,
                            User = user,
                            RoleId = Role.Id,
                            UserId = user.Id
                        });
                        user.UserRoles = userRoles;
                        var r = await _userManager.UpdateAsync(user);

                    }
                    return new
                    {
                        Token = GenerateJwtToken(model.UserName, user),
                        Succeeded = true

                    };
                }
            }
            catch
            {
                return new
                {
                    Succeeded = false

                };
            }

            throw new ApplicationException("UNKNOWN_ERROR");
        }

        private object GenerateJwtToken(string userName, User user)
        {
            
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            foreach(var role in user.UserRoles)
            {
                claims.Add(new Claim("role", role.Role.Name));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Value.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_settings.Value.JwtIssuer,
              _settings.Value.JwtIssuer,
              claims,
              expires: DateTime.UtcNow.AddMinutes(_settings.Value.JwtExpireMinutes),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();
            try
            {
                IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out SecurityToken validatedToken);
            }
            catch 
            {
                return false;
            }
            return true;
        }

        private  TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false,
                ValidateAudience = false, 
                ValidateIssuer = false,   
                ValidIssuer = _settings.Value.JwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Value.JwtKey)) 
            };
        }
    }

    


}
