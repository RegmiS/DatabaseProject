// ethnic food queries

using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class Ethnic_FoodQuery
    {
        public AppDb Db { get; }

        public Ethnic_FoodQuery(AppDb db)
        {
            Db = db;
        }

        // retrieves a specific ethnic food
        public async Task<Ethnic_Food> GetOne(string name)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getFoodEthnicity";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@fname",
                DbType = DbType.String,
                Value = name,
            });
            var result = await AllFoods(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        // retrieves all ethnic foods
        public async Task<List<Ethnic_Food>> GetAll()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getAllFood";
            cmd.CommandType = CommandType.StoredProcedure;
            return await AllFoods(await cmd.ExecuteReaderAsync());
        }

        // looks through all ethnic foods
        private async Task<List<Ethnic_Food>> AllFoods(DbDataReader reader)
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