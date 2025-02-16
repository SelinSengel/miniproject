using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using project1.Models;
using project1.Repositories;
using project1.Services;
using System.Threading.Tasks;

namespace project1.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly UserService _userService;

        // Constructor, UserRepository ve UserService ile DI
        public LoginController(UserRepository userRepository, UserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }
 
        // GET: /Login/
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
      

        // POST: /Login/
        [HttpPost]
        public async Task<IActionResult> Index(string? Username, string? Password)
        {

            //var eyp = _userService.HashPassword(Password);
            //return Ok(eyp);

            // Parametreler için null kontrolü
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ViewBag.Error = "Kullanıcı adı ve şifre boş olamaz!";
                return View();
            }

            // Kullanıcıyı veritabanından alıyoruz
            var user = await _userRepository.GetUserByUsernameAsync(Username);

            // Kullanıcı boş mu?
            if (user is null)
            {
                ViewBag.Error = "Kullanıcı bulunamadı!";
                return View();
            }

            // Kullanıcının şifre hash'i boş mu?
            if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                ViewBag.Error = "Kullanıcıya ait şifre kaydı bulunamadı!";
                return View();
            }

            // Girilen şifre doğru mu?
            if (!_userService.VerifyPassword(Password, user.PasswordHash))
            {
                ViewBag.Error = "Şifre yanlış!";
                return View();
            }
            HttpContext.Session.SetString("userid", user.UserId.ToString());
          
            TempData["SuccessMessage"] = "Giriş başarılı. Hoş geldiniz!";
            return RedirectToAction("Index", "Welcome"); 
        }

        public class LoginInfo
        {
            public string Username { get; set; }
            public string PasswordHash { get; set; }
        }
    }
}



