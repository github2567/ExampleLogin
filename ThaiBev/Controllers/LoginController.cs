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
        public async Task<IActionResult> SetToken([FromBody]string token)
        {
            if(string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            HttpContext.Session.SetString("JWToken", token);
            return Ok();
        }

        [HttpPost]
        //public async Task<IActionResult> Index([FromBody] string username, [FromBody] string password)
        public async Task<IActionResult> Index([FromBody] UserRegistration input)
        {
           
            var errMsg = "";
            if (!ModelState.IsValid)
            {
                ViewBag.inputUsername = input.UserName;
                ViewBag.Error = "Username หรือ Password ไม่ถูกต้อง";
                return View();
            }

            try
            {
                var result = await _signInManager.PasswordSignInAsync(input.UserName, input.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {

                    UserRegister model = new UserRegister();
                    model.UserName = input.UserName;
                    model.Password = input.Password;
                    model.ConfirmPassword = input.Password;
                    var response = await _httpClient.PostAsJsonAsync("http://localhost:5186/api/Authen/login", model);
                    if (response.IsSuccessStatusCode)
                    {
                        //var token = await response.Content.ReadFromJsonAsync<string>();
                        //HttpContext.Session.SetString("JWToken", token);

                        var dict = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                        string token = dict["token"];
                        HttpContext.Session.SetString("JWToken", token);
                    }



                    HttpContext.Session.SetString("Username", input.UserName);
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

            }
            catch (Exception ex)
            {

            }

            ViewBag.Error = errMsg;
            return StatusCode(500, errMsg);
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
                    string err = "";
                    if(ModelState != null && ModelState.Count() > 0)
                    {
                        err = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault().ErrorMessage;
                    }

                    return StatusCode(500, err);
                }

                var addUser = new ApplicationUser
                {
                    UserName = model.UserName
                };
                var result = await _userManager.CreateAsync(addUser, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    string err = "";
                    if (ModelState != null && ModelState.Count() > 0)
                    {
                        err = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault().ErrorMessage;
                    }
                    return StatusCode(500, err);
                }
            }
            catch (Exception ex)
            {
            
            }

            return View("Index");

        }
    }
}
