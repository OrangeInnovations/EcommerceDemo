using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.WebAPI.Services
{
    public class Settings
    {
        private readonly IConfiguration _configuration;

        public Settings(IConfiguration configuration)
        {
            _configuration = configuration;

            SqlDbConnectionString = _configuration.GetConnectionString("SqlDBConnectionString");
            ValidIssuer = _configuration["Tokens:Issuer"];
            ValidAudience = _configuration["Tokens:Audience"];
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            var disableSSLStr = _configuration["DisableSSL"];
            DisableSSL = bool.TryParse(disableSSLStr, out var disableSSL) && disableSSL;
        }

        public string SqlDbConnectionString { get; set; }
       
        public string ValidIssuer { get; private set; }
        public string ValidAudience { get; private set; }
        public SymmetricSecurityKey IssuerSigningKey { get; private set; }
        public bool DisableSSL { get; private set; }
    }
}
