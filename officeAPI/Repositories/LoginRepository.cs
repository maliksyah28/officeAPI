using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using officeAPI.Context;
using officeAPI.Models;
using officeAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace officeAPI.Repositories
{
    public class LoginRepository
    {
        public IConfiguration _configuration;
        private readonly MyContexts context;
        public LoginRepository(IConfiguration configuration, MyContexts context)
        {
            _configuration = configuration;
            this.context = context;
        }
        DynamicParameters parameters = new DynamicParameters();
        public UserTokenVM Login(LoginVM loginVM)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:officeAPI"]))
            {
                var spCheckPassword = "SP_UsersCheckPassword";
                parameters.Add("@username", loginVM.Username);
                var userPassword = connection.QuerySingleOrDefault<User>(spCheckPassword, parameters, commandType: CommandType.StoredProcedure);
                if (userPassword == null)
                {
                    return null;
                }

                //string passwordHash = BCrypt.Net.BCrypt.HashPassword(loginVM.Password);
                bool verified = BCrypt.Net.BCrypt.Verify(loginVM.Password, userPassword.Password);
                if (!verified)
                {
                    return null;
                }

                parameters = new DynamicParameters();
                var spUserToken = "SP_UsersGetLoginTokenData";
                parameters.Add("@NIK", userPassword.NIK);
                var userToken = connection.QuerySingleOrDefault<UserTokenVM>(spUserToken, parameters, commandType: CommandType.StoredProcedure);
                if (userPassword == null)
                {
                    return null;
                }

                string token = GenerateJwtToken(userToken);
                userToken.Token = token;

              

                return userToken;
            }
        }

        private string GenerateJwtToken(UserTokenVM userToken)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //new Claim(JwtRegisteredClaimNames.Sub, userToken.Username),
                //new Claim(ClaimTypes.NameIdentifier, userToken.NIK),
                //new Claim(ClaimTypes.Name, userToken.Name),
                new Claim("NIK", userToken.NIK),
                new Claim("Username", userToken.Username),
                new Claim("Name", userToken.Name),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
