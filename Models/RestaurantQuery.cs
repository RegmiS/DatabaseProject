// restaurant queries

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


        // retrieves a specific restaurant
        public async Task<Restaurant> GetOneRestaurant(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getRestaurantInformation";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rid",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await AllRestaurants(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        // retrieves all restaurants
        public async Task<List<Restaurant>> GetAll()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getRestaurants";
            cmd.CommandType = CommandType.StoredProcedure;
            return await AllRestaurants(await cmd.ExecuteReaderAsync());
        }

        // looks through all restaurants
        private async Task<List<Restaurant>> AllRestaurants(DbDataReader reader)
        {
            var all_Restaurants = new List<Restaurant>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var single_Restaurant = new Restaurant(Db)
                    {
                        restaurantID = reader.GetInt32(0),
                        address = reader.GetString(1),
                        name = reader.GetString(2),
                        priceRange = reader.GetString(3),
                        ownerID = reader.GetInt32(4),
                    };
                    all_Restaurants.Add(single_Restaurant);
                }
            }
            return all_Restaurants;
        }
    }
}