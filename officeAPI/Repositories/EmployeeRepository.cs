using Dapper;
using Microsoft.Extensions.Configuration;
using officeAPI.Context;
using officeAPI.Models;
using officeAPI.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace officeAPI.Repositories
{
    public class EmployeeRepository : IRepository<Employee, string>
    {
        public IConfiguration _configuration;
        private readonly MyContexts context;
        public EmployeeRepository(IConfiguration configuration, MyContexts context)
        {
            _configuration = configuration;
            this.context = context;
        }
        DynamicParameters parameters = new DynamicParameters();
        public int Delete(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employee> Get()
        {
            throw new NotImplementedException();
        }

        public Employee Get(string key)
        {
            throw new NotImplementedException();
        }

        public int Insert(Employee entity)
        {
            throw new NotImplementedException();
        }

        public int Update(Employee entity)
        {
            throw new NotImplementedException();
        }
    }
}
