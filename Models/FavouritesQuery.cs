using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;

namespace DatabaseProject
{
    public class FavouritesQuery
    {
        public AppDb Db { get; }

        public FavouritesQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Favourites> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `customerID`, `dateFavourited`, `restaurantID` FROM `Favourites` WHERE `customerID` = @customerID";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@customerID",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Favourites>> FindAllAsync(int customerID)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `customerID`, `dateFavourited`, `restaurantID` FROM `Favourites` WHERE `customerID` = @customerID";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@customerID",
                DbType = DbType.Int32,
                Value = customerID,
            });
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<List<Favourites>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `customerID`, `dateFavourited`, `restaurantID` FROM `Favourites` ORDER BY `customerID`";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Favourites`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Favourites>> ReadAllAsync(DbDataReader reader)
        {
            var all_customers = new List<Favourites>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var single_customer = new Favourites(Db)
                    {
                        customerID = reader.GetInt32(0),
                        dateFavourited = reader.GetDateTime(1).ToString("MM/dd/yyyy"),
                        restaurantID = reader.GetInt32(2),
                    };
                    all_customers.Add(single_customer);
                }
            }
            return all_customers;
        }
    }
}