// represents a single favourite, needs insert, updateFavourited, and delete for favourites with certain ids

using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;

namespace DatabaseProject
{
    public class Favourites
    {
        public int customerID { get; set; }
        public string dateFavourited { get; set; }
        public int restaurantID { get; set; }

        internal AppDb Db { get; set; }

        public Favourites()
        {
        }

        internal Favourites(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `Favourites` (`dateFavourited`, `restaurantID`) VALUES (@crDistance, @dateFavourited, @restaurantID);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            customerID = (int)cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `Favourites` SET `dateFavourited` = @dateFavourited, `restaurantID` = @restaurantID WHERE `customerID` = @customerID";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Favourites` WHERE `customerID` = @customerID;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@customerID",
                DbType = DbType.Int32,
                Value = customerID,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@dateFavourited",
                DbType = DbType.String,
                Value = dateFavourited,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@restaurantID",
                DbType = DbType.Int32,
                Value = restaurantID,
            });
        }

    }
}