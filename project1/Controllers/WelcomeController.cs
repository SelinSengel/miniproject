using Microsoft.AspNetCore.Mvc;

public class WelcomeController : Controller
{
    public IActionResult Index()
    {
        // Giriş yapan kullanıcının adını göstermek için ViewBag veya TempData kullanabilirsiniz.
        ViewBag.Username = User.Identity.Name; // Kullanıcı adı (Varsa Identity'den çekilir)
        return View();
    }

    // Görev sayfasına yönlendirme işlemi
    public IActionResult GoToTasks()
    {
        return RedirectToAction("Index", "Task");
    }
}
