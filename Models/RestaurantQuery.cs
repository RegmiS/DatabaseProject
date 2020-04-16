using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class RestaurantQuery
    {
        public AppDb Db { get; }

        public RestaurantQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Restaurant> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `restaurantID`, `address`, `name`, `priceRange`, `ownerID` FROM `Restaurant` WHERE `restaurantID` = @restaurantID";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@restaurantID",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Restaurant>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `restaurantID`, `address`, `name`, `priceRange`, `ownerID` FROM `Restaurant` ORDER BY `restaurantID`";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Restaurant`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Restaurant>> ReadAllAsync(DbDataReader reader)
        {
            var all_customers = new List<Restaurant>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var single_customer = new Restaurant(Db)
                    {
                        restaurantID = reader.GetInt32(0),
                        address = reader.GetString(1),
                        name = reader.GetString(2),
                        priceRange = reader.GetString(3),
                        ownerID = reader.GetInt32(4),
                    };
                    all_customers.Add(single_customer);
                }
            }
            return all_customers;
        }
    }
}