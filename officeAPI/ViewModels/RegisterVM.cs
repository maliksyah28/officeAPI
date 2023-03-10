using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace officeAPI.ViewModels
{
    public class RegisterVM
    {
        public string NIK { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
    }
    public enum Gender { Male, Female }
}

