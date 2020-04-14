using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class Has_Ethnic_FoodQuery
    {
        public AppDb Db { get; }

        public Has_Ethnic_FoodQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Has_Ethnic_Food> FindOneAsync(int restaurantID)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `restaurantID`, `foodName` FROM `has_ethnic_food` WHERE `restaurantID` = @restaurantID";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@restaurantID",
                DbType = DbType.String,
                Value = restaurantID,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Has_Ethnic_Food>> FindAllAsync(int restaurantID)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `restaurantID`, `foodName` FROM `has_ethnic_food` WHERE `restaurantID` = @restaurantID";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@restaurantID",
                DbType = DbType.Int32,
                Value = restaurantID,
            });
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<List<Has_Ethnic_Food>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `restaurantID`, `foodName` FROM `has_ethnic_food` ORDER BY `restaurantID`";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `has_ethnic_food`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Has_Ethnic_Food>> ReadAllAsync(DbDataReader reader)
        {
            var all_foods = new List<Has_Ethnic_Food>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var single_food = new Has_Ethnic_Food(Db)
                    {
                        restaurantID = reader.GetInt32(0),
                        foodName = reader.GetString(1),
                    };
                    all_foods.Add(single_food);
                }
            }
            return all_foods;
        }
    }
}