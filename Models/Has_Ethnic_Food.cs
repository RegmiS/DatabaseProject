// represents a single food property, needs insert, update, and delete for foodss with certain names

using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class Has_Ethnic_Food
    {
        public int restaurantID { get; set; }
        public string foodName { get; set; }

        internal AppDb Db { get; set; }

        public Has_Ethnic_Food()
        {
        }

        internal Has_Ethnic_Food(AppDb db)
        {
            Db = db;
        }

        public async Task InsertOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "newRestaurantFood";
            cmd.CommandType = CommandType.StoredProcedure;
            BindId(cmd);
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "deleteRestaurantFood";
            cmd.CommandType = CommandType.StoredProcedure;
            BindId(cmd);
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.String,
                Value = restaurantID,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@food",
                DbType = DbType.String,
                Value = foodName,
            });
        }

    }
}