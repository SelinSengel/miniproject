using Dapper;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using project1.Models;
using System.Runtime.InteropServices;

public class TaskRepository
{
    private readonly string _connectionString;

    public TaskRepository(IConfiguration configuration)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not found.");
    }

    // Tüm görevleri listeleme
    public async Task<IEnumerable<TaskModel>> GetAllTasksAsync(int kullanici)
    {
        if (string.IsNullOrWhiteSpace(_connectionString))
            throw new InvalidOperationException("Connection string is null or empty.");

        using (var connection = new SqlConnection(_connectionString))
        {
           
            await connection.OpenAsync();
            var query = "SELECT * FROM task WHERE KullaniciId="+kullanici.ToString()+"  ";
            var tasks = await connection.QueryAsync<TaskModel>(query);
            return tasks ?? new List<TaskModel>();
        }
    }

    // Yeni görev ekleme
    public async Task AddTaskAsync(TaskModel task)
    {
        if (task == null)
            throw new ArgumentNullException(nameof(task));

        if (string.IsNullOrWhiteSpace(_connectionString))
            throw new InvalidOperationException("Connection string is null or empty.");

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = "INSERT INTO task (KullaniciId, Aciklama, Tarih, Durum) VALUES (@KullaniciId, @Aciklama, @Tarih, @Durum)";
            await connection.ExecuteAsync(query, task);
        }
    }

    // ID'ye göre görev getirme
    public async Task<TaskModel> GetTaskByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "ID must be greater than zero.");

        if (string.IsNullOrWhiteSpace(_connectionString))
            throw new InvalidOperationException("Connection string is null or empty.");

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = "SELECT * FROM task WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<TaskModel>(query, new { Id = id })
                   ?? throw new KeyNotFoundException($"Task with ID {id} not found.");
        }
    }

    public async Task DeleteTaskAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = "DELETE FROM task WHERE Id = @Id";
            await connection.ExecuteAsync(query, new { Id = id });
        }
    }

    public async Task UpdateTaskAsync(TaskModel task)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = "UPDATE task SET Durum = @Durum WHERE Id = @Id";
            await connection.ExecuteAsync(query, task);
        }
    }

    public async Task<IEnumerable<TaskModel>> GetTasksByStatusAsync(bool durum)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var query = "SELECT * FROM task WHERE Durum = @Durum ORDER BY Tarih DESC";
            return await connection.QueryAsync<TaskModel>(query, new { Durum = durum });
        }
    }
}
