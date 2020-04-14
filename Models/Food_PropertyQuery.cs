using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class Food_PropertyQuery
    {
        public AppDb Db { get; }

        public Food_PropertyQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Food_Property> FindOneAsync(string foodName)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `foodName`, `foodProperty` FROM `food_property` WHERE `foodName` = @foodName";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@foodName",
                DbType = DbType.String,
                Value = foodName,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Food_Property>> FindAllAsync(string foodName)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `foodName`, `foodProperty` FROM `food_property` WHERE `foodName` = @foodName";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@foodName",
                DbType = DbType.String,
                Value = foodName,
            });
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<List<Food_Property>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `foodName`, `foodProperty` FROM `food_property` ORDER BY `foodName`";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `food_property`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Food_Property>> ReadAllAsync(DbDataReader reader)
        {
            var all_foods = new List<Food_Property>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var single_food = new Food_Property(Db)
                    {
                        foodName = reader.GetString(0),
                        foodProperty = reader.GetString(1),
                    };
                    all_foods.Add(single_food);
                }
            }
            return all_foods;
        }
    }
}