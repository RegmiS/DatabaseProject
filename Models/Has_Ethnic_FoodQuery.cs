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

        public async Task<List<Has_Ethnic_Food>> GetOne(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getRestaurantFoods";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            return await AllRestaurantFoods(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<Has_Ethnic_Food>> AllRestaurantFoods(DbDataReader reader)
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