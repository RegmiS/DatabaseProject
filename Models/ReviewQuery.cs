using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;

namespace DatabaseProject
{
    public class ReviewQuery
    {
        public AppDb Db { get; }

        public ReviewQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Review> GetOneReview(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getReviewInformation";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rNum",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await AllReviews(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Review>> GetAll()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getAllReviews";
            cmd.CommandType = CommandType.StoredProcedure;
            return await AllReviews(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<Review>> AllReviews(DbDataReader reader)
        {
            var all_Reviews = new List<Review>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var single_Review = new Review(Db)
                    {
                        reviewNumber = reader.GetInt32(0),
                        comment = reader.GetString(1),
                        rating = reader.GetInt32(2),
                        restaurantID = reader.GetInt32(3),
                        customerID = reader.GetInt32(4),
                        date = reader.GetDateTime(5).ToString("MM/dd/yyyy"),
                    };
                    all_Reviews.Add(single_Review);
                }
            }
            return all_Reviews;
        }
    }
}