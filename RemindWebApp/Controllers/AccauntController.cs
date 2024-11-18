using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc; 
using RemindWebApp.Models;
using RemindWebApp.ViewModels;
using RemindWebApp.DAL;
using RemindWebApp.Utlities;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace RemindWebApp.Controllers
{
   
    public class AccauntController : Controller
    {
        private RemindDatabase _reminDb;
        private UserManager<NewUser> _userManager;
        private SignInManager<NewUser> _signManager;
        private RoleManager<IdentityRole> _roleManager;
        private ILogger<AccauntController> _logger;
        private IConfiguration _configuration;
        public AccauntController (RemindDatabase reminddataBase,
            UserManager<NewUser> userManager, SignInManager<NewUser> signManager, 
            RoleManager<IdentityRole> roleManager, ILogger<AccauntController> logger,
            IConfiguration configuration)
        {
            _reminDb = reminddataBase;
            _userManager = userManager;
            _signManager = signManager;
            _roleManager = roleManager;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (registerViewModel == null)
            {
                return View(registerViewModel);
            };

            NewUser newUser = new NewUser()
            {
                Name = registerViewModel.Name,
                Surname = registerViewModel.Surname,
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email
            };

            IdentityResult identityResult = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerViewModel);
            }
          
         
            try
            {
                string emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                #region Sending Email Confirmation Message

                //SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                SmtpClient client = new SmtpClient("smtp-mail.outlook.com",587);
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_configuration["SmtpClientCredentialEmail"], _configuration["SmtpClientCredentialPassword"]);

                MailMessage message = new MailMessage(_configuration["SmtpClientCredentialEmail"], registerViewModel.Email);
                message.IsBodyHtml = true;
                message.Subject = "Emailinizi Tesdiqleyin";
                message.Body = $"<a href=`https://localhost:44370/accaunt/confirmemail?userId={newUser.Id}&token={emailConfirmationToken}`> Please Confirm Your Email password</a>";
                await client.SendMailAsync(message);

                #endregion
            }
            catch (Exception)
            {

                throw;
            }
            

            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Confirmemail(string userId, string token)
        {
            if (userId == null)
            {
                return RedirectToAction("ConfirmEmailError", "Accaunt");
            }

            if (token == null)
            {
                return RedirectToAction("ConfirmEmailError", "Accaunt");
            }

            NewUser userindb = await _userManager.FindByIdAsync(userId);

            if (userindb == null)
            {
                return RedirectToAction("ConfirmEmailError", "Accaunt");
            }

          //  await _userManager.ConfirmEmailAsync(userindb, token);
            userindb.EmailConfirmed = true;
            await _userManager.UpdateAsync(userindb);
            TempData["AccountConfirmed"] = true;


            #region Sign in
            await _signManager.SignInAsync(userindb, true);
            if (userindb.UserName.StartsWith("IamAdminRemind"))
            {
                await _userManager.AddToRoleAsync(userindb, Utility.Roles.Admin.ToString());
            }
            if (userindb.UserName.StartsWith("IamMemberRemind"))
            {
                await _userManager.AddToRoleAsync(userindb, Utility.Roles.Member.ToString());
            }
            else
            {
                await _userManager.AddToRoleAsync(userindb, Utility.Roles.Guest.ToString());
            }



            return RedirectToAction("Index", "Home");
            #endregion
        }

        public IActionResult ConfirmEmailError()
        {
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);
            NewUser find = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if (find == null)
            {
                ModelState.AddModelError("", "Email don't exist in database");
                return View(loginViewModel);
            }

            Microsoft.AspNetCore.Identity.SignInResult login = await _signManager.PasswordSignInAsync(find, loginViewModel.Password, loginViewModel.RememberMe, true);

            if (!login.Succeeded)
            {
                ModelState.AddModelError("", "Email don't exist in database");
                return View(loginViewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task RoleSeedr()
        {
            if (!await _roleManager.RoleExistsAsync(Utility.Roles.Admin.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Utility.Roles.Admin.ToString()));
            }
            if (!await _roleManager.RoleExistsAsync(Utility.Roles.Member.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Utility.Roles.Member.ToString()));
            }
            if (!await _roleManager.RoleExistsAsync(Utility.Roles.Guest.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Utility.Roles.Guest.ToString()));
            }


        }
   
    }


}
#region
//if (identityResult.Succeeded)
//{
//    var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
//    var confirmationLink = Url.Action("ConfirmEmail", "Accaunt",
//        new { userId = newUser.Id, token = token }, Request.Scheme);

//    _logger.Log(LogLevel.Warning, confirmationLink);

//    //if (_signManager.IsSignedIn(New) && )
//    //{
//    //}
//    //await _signManager.SignInAsync(newUser, isPersistent: false);
//    //return RedirectToAction("Index","Home");
//    ViewBag.ErrorTitle = "Registration succesful";
//    ViewBag.ErrorMessage = "Before you can Login, Please confirm your" + "email, by clicking on confirmaion we have emaile for you";
//    return Content("Gozle");
//}

#endregion