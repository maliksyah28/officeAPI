using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace officeAPI.ViewModels
{
    public class UserTokenVM
    {
        public string Token { get; set; } 
        public string NIK { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
