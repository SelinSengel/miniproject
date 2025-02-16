using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using project1.Models;

public class TaskController : Controller
{
    private readonly TaskService _taskService;
    public TaskController(TaskService taskService)
    {
        _taskService = taskService;
    }

    // Görevlerin listesini göster
    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("userid") != null)
        {
            // Session is not null
            var userid = HttpContext.Session.GetString("userid");
           
            var tasks = await _taskService.GetAllTasksAsync(Convert.ToInt32( userid));

            return View(tasks);
        }
        return View();
    }

    // Yeni görev eklemek için form göster
    public IActionResult Create()
    {
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }

    // Yeni görev ekleme işlemi
    [HttpPost]
    public async Task<IActionResult> Create(TaskModel task)
    {
        if (ModelState.IsValid)
        {
            await _taskService.AddTaskAsync(task);
            return RedirectToAction(nameof(Index));
        }
        return View(task);
    }

    // Görev detaylarını göster
    public async Task<IActionResult> Details(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }

    [HttpPost]
    [Route("Task/Add")]
    public async Task<IActionResult> Add(string aciklama)
    {
        if (string.IsNullOrWhiteSpace(aciklama))
        {
            ModelState.AddModelError("", "Açıklama boş olamaz.");
            return RedirectToAction(nameof(Index));
        }
        var userid = HttpContext.Session.GetString("userid");
        var newTask = new TaskModel
        {
            
            KullaniciId = Convert.ToInt32( userid), // Varsayılan bir kullanıcı ID'si veya oturumdan alınabilir
            Aciklama = aciklama,
            Tarih = DateTime.Now,
            Durum = false
        };

        await _taskService.AddTaskAsync(newTask);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Route("Task/Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        // Silme işlemini yap
        await _taskService.DeleteTaskAsync(id);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Route("Task/Complete")]
    public async Task<IActionResult> Complete(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        // Tamamlandı olarak işaretle
        task.Durum = true;
        await _taskService.UpdateTaskAsync(task);

        return RedirectToAction(nameof(Index));
    }

    // Panoya gitmek için, tamamlanan ve devam eden görevleri al
    public async Task<IActionResult> PanoyaGit()
    {
        var completedTasks = await _taskService.GetTasksByStatusAsync(true); // Tamamlanan görevler
        var ongoingTasks = await _taskService.GetTasksByStatusAsync(false); // Devam eden görevler

        var viewModel = new PanoyaGitModel
        {
            CompletedTasks = completedTasks,
            OngoingTasks = ongoingTasks
        };

        return View(viewModel);
    }



}
