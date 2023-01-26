﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using officeAPI.Repositories;
using officeAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace officeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginRepository repository;
        public LoginController(LoginRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        public virtual ActionResult Insert(LoginVM loginVM)
        {
            var insert = repository.Login(loginVM);
            if (insert != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Berhasil Login", Data = insert });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Login, username atau password salah.", Data = insert });
            }
        }
    }
}
