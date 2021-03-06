//favourites queries

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

        // retreives favourites for one customer
        public async Task<List<Favourites>> GetOne(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getCustomerFavourites";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            return await AllFavourites(await cmd.ExecuteReaderAsync());
        }

        // looks through all favourites
        private async Task<List<Favourites>> AllFavourites(DbDataReader reader)
        {
            var all_customers = new List<Favourites>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var single_customer = new Favourites(Db)
                    {
                        customerID = reader.GetInt32(0),
                        restaurantID = reader.GetInt32(1),
                        dateFavourited = reader.GetDateTime(2).ToString("MM/dd/yyyy"),
                    };
                    all_customers.Add(single_customer);
                }
            }
            return all_customers;
        }
    }
}