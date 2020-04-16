// represents a single review

using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;

namespace DatabaseProject
{
    public class Review
    {
        public int reviewNumber { get; set; }
        public string comment { get; set; }
        public int rating { get; set; }
        public int restaurantID { get; set; }
        public int customerID { get; set; }
        public string date { get; set; }

        internal AppDb Db { get; set; }

        public Review()
        {
        }

        internal Review(AppDb db)
        {
            Db = db;
        }

        // insert a new reivew
        public async Task InsertOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "newReview";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rComment",
                DbType = DbType.String,
                Value = comment,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rRating",
                DbType = DbType.Int32,
                Value = rating,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rid",
                DbType = DbType.Int32,
                Value = restaurantID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@cid",
                DbType = DbType.Int32,
                Value = customerID,
            });
            await cmd.ExecuteNonQueryAsync();
            reviewNumber = (int)cmd.LastInsertedId;
        }

        // binds review number
        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rNum",
                DbType = DbType.Int32,
                Value = reviewNumber,
            });
        }

        // binds parameters
        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rComment",
                DbType = DbType.String,
                Value = comment,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rRating",
                DbType = DbType.Int32,
                Value = rating,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rid",
                DbType = DbType.Int32,
                Value = restaurantID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@cid",
                DbType = DbType.Int32,
                Value = customerID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rDate",
                DbType = DbType.String,
                Value = date,
            });
        }

    }
}