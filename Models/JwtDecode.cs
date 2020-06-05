using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public static class JwtDecode
    {
        public static string User(string token)
        {
      
            var jwtEncodedString = token.Replace("Bearer ", string.Empty); // trim 'Bearer ' from the start since its just a prefix for the token string

            var decodeToken = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);
            var userName = decodeToken.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;
            return userName;
        }
    }
}
