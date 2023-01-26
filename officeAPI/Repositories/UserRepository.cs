using Dapper;
using Microsoft.Extensions.Configuration;
using officeAPI.Context;
using officeAPI.Models;
using officeAPI.Repositories.Interface;
using officeAPI.ViewModels;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Text.RegularExpressions;

namespace officeAPI.Repositories
{
    public class UserRepository : IRepository<User, string>
    {
        public IConfiguration _configuration;
        private readonly MyContexts context;
        public UserRepository(IConfiguration configuration, MyContexts context)
        {
            _configuration = configuration;
            this.context = context;
        }
        DynamicParameters parameters = new DynamicParameters();

        //get ALL data
        public virtual IEnumerable<UserEmployeeVM> GetAllUserEmployee()
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:officeAPI"]))
            {
                var spName = "SP_EmployeesGetAll";
                var res = connection.Query<UserEmployeeVM>(spName, parameters, commandType: CommandType.StoredProcedure);
                return res;
            }
        }

        public virtual int InsertUserEmployee(UserEmployeeVM userEmployeeVM)
        {
            using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:officeAPI"]))
            {
                //data dibuat logic nya di API (logic ada dibawah)
                string generatedNIK = GenerateNIK();
                // data dibuat logic nya di API (logic ada di bawah)
                string generatedPassword = GeneratePassword(userEmployeeVM.Username);
                if (generatedPassword == "") return -1;
                // buat createdAT 
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                // buat hash password
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(generatedPassword);

                var spName = "SP_UsersEmployeeInsert";
                parameters = new DynamicParameters();
                parameters.Add("@NIK", generatedNIK);
                parameters.Add("@Username", userEmployeeVM.Username);
                parameters.Add("@Password", passwordHash);
                parameters.Add("@Name", userEmployeeVM.Name);
                parameters.Add("@Email", userEmployeeVM.Email);
                parameters.Add("@BirthDate", userEmployeeVM.BirthDate);
                parameters.Add("@Gender", userEmployeeVM.Gender);
                parameters.Add("@Phone", userEmployeeVM.Phone);
                parameters.Add("@Address", userEmployeeVM.Address);
                parameters.Add("@DepartmentId", userEmployeeVM.DepartmentId);
                parameters.Add("@CreatedAt", time);
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

        public int Delete(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> Get()
        {
            throw new NotImplementedException();
        }

        public User Get(string key)
        {
            throw new NotImplementedException();
        }

        public int Insert(User entity)
        {
            throw new NotImplementedException();
        }

        public int Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
