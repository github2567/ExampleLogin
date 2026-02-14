using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using ThaiBev.DAL.Common;
using ThaiBev.DAL.Data;
using ThaiBev.Domain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ThaiBev.Controllers
{
	[AllowAnonymous]
	public class LoginController : Controller
    {
        private readonly ThaiBevDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IWebHostEnvironment _env;
        private readonly HttpClient _httpClient;

        [ActivatorUtilitiesConstructor]
        public LoginController(ThaiBevDbContext dbContext, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IWebHostEnvironment env, 
            IHttpClientFactory httpClientFactory)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _env = env;
            _httpClient = httpClientFactory.CreateClient();
        }

		[HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
           
            var errMsg = "";
            if (!ModelState.IsValid)
            {
                ViewBag.inputUsername = username;
                ViewBag.Error = "Username หรือ Password ไม่ถูกต้อง";
                return View();
            }

            var result =
                await _signInManager.PasswordSignInAsync(username, password, isPersistent: false,
                    lockoutOnFailure: false);

            if (result.Succeeded)
            {

                UserRegister model = new UserRegister();
                model.UserName = username;
                model.Password = password;
                model.ConfirmPassword = password;
                var response = await _httpClient.PostAsJsonAsync("http://localhost:5147/api/Authen/login", model);
                if (response.IsSuccessStatusCode)
                {
                    //var token = await response.Content.ReadFromJsonAsync<string>();
                    //HttpContext.Session.SetString("JWToken", token);

                    var dict = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                    string token = dict["token"];
                    HttpContext.Session.SetString("JWToken", token);
                }



                HttpContext.Session.SetString("Username", username);
                return RedirectToAction("Index", "Home");
            }

           
            if (result.IsLockedOut)
            {
                errMsg = "Is Locked Out";
            }

            if (result.IsNotAllowed)
            {
                errMsg = "Is Not Allowed";
            }

            if (result.RequiresTwoFactor)
            {
                errMsg = "Requires Two Factor.";
            }

            if (errMsg == "")
            {
                errMsg = "Username หรือ Password ไม่ถูกต้อง";
            }

            ViewBag.Error = errMsg;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        public async Task<IActionResult> ConfirmRegister([FromBody] UserRegister model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    foreach (var error in ModelState)
                    {
                        if (error.Value.Errors.Count > 0)
                        {
                            ModelState.AddModelError(error.Key, error.Value.Errors[0].ErrorMessage);
                        }
                    }

                }

                var user = new ApplicationUser { UserName = model.UserName };

                var passwordValidators = _userManager.PasswordValidators;
                var validationErrors = new List<IdentityError>();

                if (!string.IsNullOrEmpty(model.Password))
                {
                    foreach (var validator in passwordValidators)
                    {
                        var resultError = await validator.ValidateAsync(_userManager, user, model.Password);
                        if (!resultError.Succeeded)
                        {
                            validationErrors.AddRange(resultError.Errors);
                        }
                    }

                    if (validationErrors.Any())
                    {
                        foreach (var error in validationErrors)
                        {
                            ModelState.AddModelError("Password", error.Description);
                        }
                    }
                }

                if (!ModelState.IsValid)
                {
                    return PartialView("Register", model);
                }

                var addUser = new ApplicationUser
                {
                    UserName = model.UserName
                };
                var result = await _userManager.CreateAsync(addUser, model.Password);

                if (result.Succeeded)
                {
                    return View("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return PartialView("Register", model);
                }
            }
            catch (Exception ex)
            {
            
            }

            return View("Index");

        }
    }
}
