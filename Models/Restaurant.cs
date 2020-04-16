// represents a single restaurant, needs insert, update, and delete for restaurants with certain ids

using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class Restaurant
    {
        public int restaurantID { get; set; }
        public string address { get; set; }
        public string name { get; set; }
        public string priceRange { get; set; }
        public int ownerID { get; set; }

        internal AppDb Db { get; set; }

        public Restaurant()
        {
        }

        internal Restaurant(AppDb db)
        {
            Db = db;
        }

        public async Task InsertOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "newRestaurant";
            cmd.CommandType = CommandType.StoredProcedure;
            BindId(cmd);
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            restaurantID = (int) cmd.LastInsertedId;
        }

        public async Task UpdateOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "updaterestaurantInformation";
            cmd.CommandType = CommandType.StoredProcedure;
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rid",
                DbType = DbType.Int32,
                Value = restaurantID,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rAddress",
                DbType = DbType.String,
                Value = address,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rName",
                DbType = DbType.String,
                Value = name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rPrice",
                DbType = DbType.String,
                Value = priceRange,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rOwner",
                DbType = DbType.Int32,
                Value = ownerID,
            });
        }

    }
}