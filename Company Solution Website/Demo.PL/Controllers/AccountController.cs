using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<ApplicationUser> UserManager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        #region Sign Up 
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Email = model.Email,
                    UserName = model.Email.Split('@')[0],
                    IsAgree = model.IsAgree
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                    return RedirectToAction(nameof(SignIn));
                foreach(var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
        #endregion

        #region Sign In
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);

                if(user != null)
                {
                    var password = await UserManager.CheckPasswordAsync(user, model.Password);
                    if(password)
                    {
                        var result = await SignInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                    
                    ModelState.AddModelError("", "Invalid Password");

                }
                ModelState.AddModelError("", "Invalid Email");

            }
            return View(model);

        }
        #endregion

        #region Sign Out
        public async new Task<IActionResult> SignOut()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        #endregion

        #region Forget Password

        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    var token = await UserManager.GeneratePasswordResetTokenAsync(user);
                    var resetPasswordLink = Url.Action("ResetPassword", "Account", new { Email = model.Email, Token = token }, Request.Scheme);

                    var email = new Email()
                    {
                        Title = "Reset Password",
                        Body = resetPasswordLink, 
                        To = model.Email
                    };
                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CompleteForgetPassword));
                }
                ModelState.AddModelError(string.Empty, "Email is not Existed");
            }
            return View(model);
        }
        public IActionResult CompleteForgetPassword()
        {
            return View();
        }
        #endregion

        #region Reset Password

        public IActionResult ResetPassword(string email, string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    var result = await UserManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if(result.Succeeded)
                        return RedirectToAction(nameof(ResetPasswordDone));
                    foreach (var Error in result.Errors)
                        ModelState.AddModelError(string.Empty, Error.Description);

                }
            }
            return View(model);
        }

        public IActionResult ResetPasswordDone()
        {
            return View();
        }
        #endregion
    }
}
