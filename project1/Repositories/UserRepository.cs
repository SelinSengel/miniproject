
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using project1.Models;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace project1.Repositories
{
    public class UserRepository
    {
        private readonly IDbConnection _dbConnection;
        private object _connection;

        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            const string query = @"
            SELECT 
            id AS UserId, 
            kullaniciadi AS Username, 
            sifre AS PasswordHash
            FROM userlogin
            WHERE kullaniciadi = @Username";

            // Parametre eşleştirme
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(
                query,
                new { Username = username }
            );
        }


        public async Task<int?> GetUserIdByUsername(string username)
        {
             string sorgu = @" SELECT 
            id AS UserId
             FROM userlogin
            WHERE kullaniciadi = "+username;
            return await _dbConnection.QueryFirstOrDefaultAsync<int?>(sorgu);
        }

       
        private IActionResult Ok(IEnumerable<UserLoginInfo> userinfo)
        {
            throw new NotImplementedException();
        }


        [HttpGet("LoginInfo")]
        public async Task<IEnumerable<User>> GetAll()
        {
            var query = "SELECT id AS UserId, kullaniciadi AS Username, sifre AS PasswordHash FROM userlogin";
            return await _dbConnection.QueryAsync<User>(query);
        }

        [HttpPost("GetUserById")]
        public async Task<IEnumerable<User>> GetUserById(int id)
        {
            var query = "SELECT id AS UserId,kullaniciadi AS Username, sifre AS PasswordHash FROM userlogin WHERE id=@id";
            return await _dbConnection.QueryAsync<User>(query, new {id});
        }



    }
}


