using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using officeAPI.Models;
namespace officeAPI.ViewModels
{
    public class UserEmployeeVM
    {
        public string NIK { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
