using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using officeAPI.Context;
using Dapper;
using officeAPI.ViewModels;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace officeAPI.Repositories
{
    public class RegisterRepository
    {
       public IConfiguration _configuration;
        private readonly MyContexts context;
        public RegisterRepository (IConfiguration configuration, MyContexts context)
        {
            _configuration = configuration;
            this.context = context;
        }
        DynamicParameters parameters = new DynamicParameters();
        public virtual int Register(RegisterVM registerVM)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:officeAPI"]))
            {
                //data dibuat logic nya di API (logic ada dibawah)
                string generatedNIK = GenerateNIK();
                // data dibuat logic nya di API (logic ada di bawah)
                string generatedPassword = GeneratePassword(registerVM.Username);
                if (generatedPassword == "") return -1;
              
                // buat hash password
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(generatedPassword);

                var spName = "SP_RegisterUser";
                parameters = new DynamicParameters();
                parameters.Add("@NIK", generatedNIK);
                parameters.Add("@Username", registerVM.Username);
                parameters.Add("@Name", registerVM.Name);
                parameters.Add("@Password", passwordHash);
                parameters.Add("@Email", registerVM.Email);
                parameters.Add("@BirthDate", registerVM.BirthDate);
                parameters.Add("@Gender", registerVM.Gender);
                parameters.Add("@Phone", registerVM.Phone);       
                var insert = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
                return insert;
            }
        }

        //Logic auto input NIK
        private string GenerateNIK()
        {
            var lastId = context.Users.FromSqlRaw(
                "SELECT TOP 1 * " +
                "FROM Users " +
                "WHERE len(NIK) = 12 " +
                "ORDER BY RIGHT(NIK, 4) desc"
                ).ToList();
            int highestId = 0;
            if (lastId.Any())
            {
                var newId = lastId[0].NIK;
                newId = newId.Substring(newId.Length - 4);
                highestId = Convert.ToInt32(newId);
            }

            int increamentId = highestId + 1;
            string generatedNIK = increamentId.ToString().PadLeft(4, '0');
            DateTime today = DateTime.Today;
            var dateNow = today.ToString("yyyyddMM");
            generatedNIK = dateNow + generatedNIK;

            return generatedNIK;
        }

        //Logic auto input Password
        private string GeneratePassword(string username)
        {
            string generatedPassword = "";
            string number = "654321";
            Match match = Regex.Match(username, @"\d+");
            if (match.Success)
            {
                number = match.Value.ToString() + number;
            }

            string[] splittedName = username.Split('.');
            if (splittedName.Length > 1)
            {
                generatedPassword = splittedName[0].Substring(0, 1)[0].ToString().ToUpper() + splittedName[1].Substring(0, 1)[0].ToString().ToUpper() + number;
            }
            else if (splittedName.Length == 1)
            {
                generatedPassword = splittedName[0].Substring(0, 1)[0].ToString().ToUpper() + number;
            }

            return generatedPassword;
        }
    }
}
