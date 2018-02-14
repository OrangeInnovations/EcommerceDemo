using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Core.Entities;
using ECommerce.WebAPI.Services;
using ECommerce.WebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.WebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly Settings _settings;
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<StoreUser> _signInManager;
        private readonly UserManager<StoreUser> _userManager;

        public AccountController(ILogger<AccountController> logger,SignInManager<StoreUser> signInManager,
            UserManager<StoreUser> userManager,Settings settings)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _settings = settings;
        }

        /// <summary>Create a token 
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                    if (result.Succeeded)
                    {
                        // Create the token
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Sid,user.Id), 
                        };
                       
                        var creds = new SigningCredentials(_settings.IssuerSigningKey, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            _settings.ValidIssuer,
                            _settings.ValidAudience,
                            claims,
                            expires: DateTime.Now.AddMinutes(30),
                            signingCredentials: creds);

                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return Created("", results);
                    }
                }
            }

            return BadRequest();
        }

    }
}