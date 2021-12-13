using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MedHelper_API.Config;
using MedHelper_API.Responses;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;


namespace JwtAuth
{
    public class JwtAuthManager : IJwtAuthManager
    {
        private readonly JwtTokenConfig _jwtTokenConfig;

        public JwtAuthManager(JwtTokenConfig jwtTokenConfig)
        {
            _jwtTokenConfig = jwtTokenConfig;
        }

        public Task<AuthResponse> GenerateTokens(List<Claim> claims)
        {
            var now = DateTime.Now;
            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x =>
                x.Type == JwtRegisteredClaimNames.Aud)?.Value);
            
            var jwtToken = new JwtSecurityToken(
                _jwtTokenConfig.Issuer,
                shouldAddAudienceClaim ? _jwtTokenConfig.Audience : string.Empty,
                claims,
                expires: now.AddMinutes(_jwtTokenConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new 
                        SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtTokenConfig.SecretKey)),
                    SecurityAlgorithms.HmacSha256Signature));
            
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            
            return Task.FromResult(new AuthResponse
            {
                AccessToken = accessToken
            });
        }
    }
}