using AutoMapper;
using BudgetManager.Models.Entities;
using BudgetManager.Models.ViewModels;
using BudgetManager.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace BudgetManager.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        public UserController(UserManager<User> userManager, IMapper mapper,
            SignInManager<User> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RecoverPassword(string? code = null)
        {
            if(code is null)
            {
                var message = "Código no encontrado";
                return RedirectToAction("ForgotPassword", new {message});
            }

            var model = new RecoverPasswordViewModel()
            {
                RecoverCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            } 

            var user = await _userManager.FindByEmailAsync(viewModel.Email);

            if(user is null)
            {
                return RedirectToAction("PasswordChanged");
            }

            var result = await _userManager.ResetPasswordAsync(user, viewModel.RecoverCode, viewModel.Password);
            return RedirectToAction("PasswordChanged");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PasswordChanged()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var message = "Proceso concluido. Si el email dado se corresponde con uno de nuestros usuarios, " +
                "en su bandeja de entrada podrá encontrar las instrucciones para recuperar su contraseña.";

            ViewBag.Message = message;
            ModelState.Clear();

            var user = await _userManager.FindByEmailAsync(viewModel.Email);

            if(user is null)
            {
                return View();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var base64Code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var link = Url.Action("RecoverPassword", "User", new {code = base64Code}, protocol: Request.Scheme);

            await _emailService.SendEmailForgotPassword(user.Email, link!);
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = _mapper.Map<User>(viewModel);

            var result = await _userManager.CreateAsync(user, password: viewModel.Password);

            if(result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                return RedirectToAction("Index", "Transaction");
            } 
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("LogIn");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LogInViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(viewModel.Email,
                viewModel.Password, viewModel.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded) 
            {
                return RedirectToAction("Index", "Transaction");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View(viewModel);
            }

        }
    }
}
