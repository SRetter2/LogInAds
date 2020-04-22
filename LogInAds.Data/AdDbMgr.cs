using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LogInAds.Data
{
    public class AdDbMgr
    {
        private string _connectionString;
        public AdDbMgr(string conn)
        {
            _connectionString = conn;
        }
        public List<Ad> GetAllAds()
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "Select * From Ad";
                conn.Open();
                var result = new List<Ad>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new Ad
                    {
                        Id=(int)reader["Id"],
                        Title = (string)reader["Title"],
                        Number = (string)reader["Number"],
                        Description = (string)reader["Description"],
                        UserId = (int)reader["UserId"]
                    });
                }
                return result;
            }
        }
        public void AddAd(Ad ad)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "Insert Into Ad(Title,Description, Number, UserId)Values(@title, @descrip, @num, @id)";
                cmd.Parameters.AddWithValue("@title", ad.Title);
                cmd.Parameters.AddWithValue("@descrip", ad.Description);
                cmd.Parameters.AddWithValue("@num", ad.Number);
                cmd.Parameters.AddWithValue("@id", ad.UserId);
                conn.Open();
                cmd.ExecuteNonQuery();

            }
        }
        public void DeleteAd(int id)
        {
            using(var conn = new SqlConnection(_connectionString))
                using(var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "Delete From Ad where Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<Ad> GetAdsForUser(int userId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "Select * From Ad Where UserId = @id ";
                cmd.Parameters.AddWithValue("@id", userId);
                conn.Open();
                var result = new List<Ad>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new Ad
                    {
                        Id = (int)reader["Id"],
                        Title = (string)reader["Title"],
                        Number = (string)reader["Number"],
                        Description = (string)reader["Description"],
                        UserId = (int)reader["UserId"]
                    });
                }
                return result;
            }
        }
    }
}
