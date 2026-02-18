using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using ThaiBev.DAL.Common;
using ThaiBev.DAL.Data;
using ThaiBev.Domain.Models;

namespace ThaiBev.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly HttpClient _httpClient;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(IWebHostEnvironment hostingEnvironment, 
            ILogger<HomeController> logger, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;

            _hostingEnvironment = hostingEnvironment;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userName = User.Identity.Name;
            ViewBag.Name = "Welcome User : " + userName;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
