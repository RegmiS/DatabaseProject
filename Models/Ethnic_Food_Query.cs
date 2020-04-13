using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class Ethnic_Food_Query
    {
        public AppDb Db { get; }

        public Ethnic_Food_Query(AppDb db)
        {
            Db = db;
        }

        public async Task<Ethnic_Food> FindOneAsync(string id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `name`, `ethnicity` WHERE `name` = @name";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@name",
                DbType = DbType.String,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Ethnic_Food>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `name`, `ethnicity` FROM `ethnic_food` ORDER BY `name`";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `ethnic_food`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Ethnic_Food>> ReadAllAsync(DbDataReader reader)
        {
            var all_foods = new List<Ethnic_Food>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var single_food = new Ethnic_Food(Db)
                    {
                        name = reader.GetString(0),
                        ethnicity = reader.GetString(1),
                    };
                    all_foods.Add(single_food);
                }
            }
            return all_foods;
        }
    }
}