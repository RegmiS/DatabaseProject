//food property queries

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

        // retrieve the food properties for a specific food
        public async Task<List<Food_Property>> GetOne(string fname)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getFoodProperties";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@fname",
                DbType = DbType.String,
                Value = fname,
            });
            return await AllFoodProperties(await cmd.ExecuteReaderAsync());
        }

        // retrieves all food properties 
        public async Task<List<Food_Property>> GetAll()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getAllFoodProperties";
            cmd.CommandType = CommandType.StoredProcedure;
            return await AllFoodProperties(await cmd.ExecuteReaderAsync());
        }

        //looks through all food properties
        private async Task<List<Food_Property>> AllFoodProperties(DbDataReader reader)
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