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

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `Restaurant` (`address`, `name`, `priceRange`, `ownerID`) VALUES (@address, @name, @priceRange, @ownerID);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            restaurantID = (int)cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `Restaurant` SET `address` = @address, `name` = @name, `priceRange` = @priceRange, `ownerID` = @ownerID WHERE `restaurantID` = @restaurantid";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Restaurant` WHERE `restaurantID` = @restaurantid;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@restaurantid",
                DbType = DbType.Int32,
                Value = restaurantID,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@address",
                DbType = DbType.String,
                Value = address,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@name",
                DbType = DbType.String,
                Value = name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@priceRange",
                DbType = DbType.String,
                Value = priceRange,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ownerID",
                DbType = DbType.Int32,
                Value = ownerID,
            });
        }

    }
}