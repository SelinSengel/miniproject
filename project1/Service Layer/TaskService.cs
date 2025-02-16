using Microsoft.Data.SqlClient;
using project1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TaskService
{
    private readonly TaskRepository _taskRepository;

    public TaskService(TaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    // Tüm görevleri listeleme
    public async Task<IEnumerable<TaskModel>> GetAllTasksAsync(int kullanici)
    {
        return await _taskRepository.GetAllTasksAsync(kullanici);
    }


    // Yeni görev ekleme
    public async Task AddTaskAsync(TaskModel task)
    {
        await _taskRepository.AddTaskAsync(task);
    }

    // ID'ye göre görev getirme
    public async Task<TaskModel> GetTaskByIdAsync(int id)
    {
        return await _taskRepository.GetTaskByIdAsync(id);
    }

    public async Task DeleteTaskAsync(int id)
    {
        await _taskRepository.DeleteTaskAsync(id);
    }

    public async Task UpdateTaskAsync(TaskModel task)
    {
        await _taskRepository.UpdateTaskAsync(task);
    }

    // Duruma göre görevleri getirme
    public async Task<IEnumerable<TaskModel>> GetTasksByStatusAsync(bool durum)
    {
        return await _taskRepository.GetTasksByStatusAsync(durum);
    }
}
