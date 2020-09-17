using HeplerLibs.ExtLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model.Jwt;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LifeHepler.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        private readonly JwtModel _jwtModel;
        public JwtController(IOptions<JwtModel> jwtModel)
        {
            _jwtModel = jwtModel.Value;
        }

        /// <summary>
        /// 取得Token
        /// </summary>
        [HttpPost("GetToken")]
        [AllowAnonymous]
        public ActionResult GetToken([FromForm] GetTokenModel data)
        {
            if (data.Pwd != _jwtModel.Pwd)
                return Forbid();

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtModel.Key));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _jwtModel.Issuer,
                audience: _jwtModel.Audience,
                claims: claims,
                expires: GetTime.UtcNow.AddMinutes(1),
                signingCredentials: credentials);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new ContentResult { Content = jwtToken };
        }

    }
}
