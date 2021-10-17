using AngularToAPI.Services;
using AngularToApiw.Models;
using AngularToApiw.ModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

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

                    //http://localhost:5000/Account/Registrationconfirm?ID&Token=4511785
                    // create token

                    var token = await _manager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmeLink = Url.Action("Registrationconfirm", "Account", new
                    {
                        ID = user.Id,
                        Token = HttpUtility.UrlEncode(token)
                    }, Request.Scheme);
                    var txt = "Please confirm your  Registration at out sute";
                    var link = "<a href=\"" + confirmeLink + "\">Registration confirm</a>";
                    var title = "Registration Confirm";
                    if (await SendGridAPI.Execute(user.Email, user.UserName, txt, link, title))
                    {
                        return Ok("Registration cooomplete");
                    }

                 
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

        [HttpGet]
        [Route("RegistrationConfirm")]
        public async Task<IActionResult> RegistrationConfirm(string ID, string Token)
        {
            if (string.IsNullOrEmpty(ID) || string.IsNullOrEmpty(Token))
                return NotFound();
            var user = await _manager.FindByIdAsync(ID);
            if (user == null)
                return NotFound();

            var result = await _manager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(Token));
            if (result.Succeeded)
            {
                return Ok("Registration succes");
            }
            else
            {
                return Ok("RegistratiSSSon succes");
            }
        }
    }
}
