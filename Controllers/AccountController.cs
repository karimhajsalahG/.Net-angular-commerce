using AngularToApiw.Models;
using AngularToApiw.ModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AngularToApiw.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDB _db;
        private readonly UserManager<ApplicationUser> _manager;

        public AccountController(ApplicationDB db, UserManager<ApplicationUser> manage)
        {
            _db = db;
            _manager = manage;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (model == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (EmailExistes(model.Email))
                {
                    return BadRequest("Email is used");
                }
                if (!IsEmailValid(model.Email))
                {
                    return BadRequest("Email not valid!!");
                }
                if (UserNameExistes(model.UserName))
                {
                    return BadRequest("UserName is used");
                }
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.UserName

                };
                // saved in data base
                var result = await _manager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK);
                }
                else
                {
                    return BadRequest(result.Errors);
                }

            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        private bool UserNameExistes(string userName)
        {
            return _db.Users.Any(x => x.UserName == userName);
        }

        private bool EmailExistes(string email)
        {
            return _db.Users.Any(x => x.Email == email);
        }
        private bool IsEmailValid(string email)
        {
            Regex em = new Regex(@"\w+\@\w+.com|\w+\@\w+.net");
            if (em.IsMatch(email))
            {
                return true;
            }
            return false;
        }
    }
}
