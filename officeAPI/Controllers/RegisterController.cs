using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using officeAPI.Repositories;
using officeAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace officeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly RegisterRepository repository;
        public RegisterController (RegisterRepository repository)
        {
            this.repository = repository;
        }
        public virtual ActionResult Insert(RegisterVM registerVM)
        {
            var insert = repository.Register(registerVM);
            if (insert != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Berhasil Register", Data = insert });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Register.", Data = insert });
            }
        }

    }
}
