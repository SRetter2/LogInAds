using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LogInAds.Data
{
   public class PasswordDbMgr
    {
        private string _connectionString;
        public PasswordDbMgr(string conn)
        {
            _connectionString = conn;
        }
        public int AddUser(string email, string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            using(var conn= new SqlConnection(_connectionString))
                using(var cmd =conn.CreateCommand())
            {
                cmd.CommandText = "Insert Into LogInUsers(Email, PasswordHash)Values(@email, @hash) Select Scope_Identity()";
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@hash", hash);
                conn.Open();
                return (int)(decimal)cmd.ExecuteScalar();
            }
        }
        public User Login(string email, string password)
        {
            var user = GetUserByEmail(email);
            if(user == null)
            {
                return null;
            }
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (isValidPassword)
            {
                return user;
            }
            return null;
        }
        public User GetUserByEmail(string email)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "Select * From LogInUsers Where Email = @email";
                cmd.Parameters.AddWithValue("@email", email);
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    return null;
                }
                return new User
                {
                    Id = (int)reader["Id"],
                    Email = (string)reader["Email"],
                    PasswordHash = (string)reader["PasswordHash"]
                };
            }
        }
    }
}
