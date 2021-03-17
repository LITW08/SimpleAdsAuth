using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SimpleAdsAuth.Data
{
    public class SimpleAdDb
    {
        private readonly string _connectionString;

        public SimpleAdDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddSimpleAd(SimpleAd ad)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Ads (Description, PhoneNumber, DateCreated, UserId) " +
                                  "VALUES (@desc, @phone, GETDATE(), @userId) SELECT SCOPE_IDENTITY()";
            command.Parameters.AddWithValue("@desc", ad.Description);
            command.Parameters.AddWithValue("@phone", ad.PhoneNumber);
            command.Parameters.AddWithValue("@userId", ad.UserId);
            connection.Open();
            ad.Id = (int)(decimal)command.ExecuteScalar();
        }

        public List<SimpleAd> GetAds()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT a.*, u.Name FROM Ads a " +
                                  "JOIN Users u on a.UserId = u.Id " +
                                  "ORDER BY a.DateCreated DESC";
            connection.Open();
            List<SimpleAd> ads = new List<SimpleAd>();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ads.Add(GetAdFromReader(reader));
            }

            return ads;
        }

        public List<SimpleAd> GetAdsForUser(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT a.*, u.Name FROM Ads a " +
                                  "JOIN Users u on a.UserId = u.Id " +
                                  "WHERE a.UserId = @userId " +
                                  "ORDER BY a.DateCreated DESC";
            command.Parameters.AddWithValue("@userid", userId);
            connection.Open();
            List<SimpleAd> ads = new List<SimpleAd>();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ads.Add(GetAdFromReader(reader));
            }

            return ads;
        }

        public int GetUserIdForAd(int adId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT UserId FROM Ads WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", adId);
            connection.Open();
            return (int)cmd.ExecuteScalar();
        }

        public void Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Ads WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            command.ExecuteNonQuery();
        }

        private SimpleAd GetAdFromReader(SqlDataReader reader)
        {
            var ad = new SimpleAd
            {
                Description = reader.Get<string>("Description"),
                Date = reader.Get<DateTime>("DateCreated"),
                PhoneNumber = reader.Get<string>("PhoneNumber"),
                Id = reader.Get<int>("Id"),
                UserId = reader.Get<int>("UserId"),
                PosterName = reader.Get<string>("Name")
            };
            return ad;
        }
    }
}