using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class AuthController : Controller
    {
        private string _userId;

        public string CurrentUserId
        {
            get
            {
                if (string.IsNullOrEmpty(_userId))
                {
                    _userId = GetUserId();
                }

                return _userId;
            }
        }

        private string GetUserId()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var claim = claims.First(c => c.Type == JwtRegisteredClaimNames.Sid);
            return claim.Value;
        }
    }
}