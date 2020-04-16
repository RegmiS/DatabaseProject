// represents a single favourite

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

        // insert new favourite
        public async Task InsertOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "newFavourite";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = customerID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@restaurant",
                DbType = DbType.Int32,
                Value = restaurantID,
            });
            await cmd.ExecuteNonQueryAsync();
            customerID = (int)cmd.LastInsertedId;
        }

        // delete an existing favourite
        public async Task DeleteOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "deleteCustomerFavourite";
            cmd.CommandType = CommandType.StoredProcedure;
            BindId(cmd);
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@restaurant",
                DbType = DbType.Int32,
                Value = restaurantID,
            });
            await cmd.ExecuteNonQueryAsync();
        }

        // binds customer ID
        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = customerID,
            });
        }

        // binds parameters
        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@restaurant",
                DbType = DbType.Int32,
                Value = restaurantID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@dateFavourited",
                DbType = DbType.String,
                Value = dateFavourited,
            });
        }

    }
}