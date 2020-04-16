// represents a single food property

using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class Food_Property
    {
        public string foodName { get; set; }
        public string foodProperty { get; set; }

        internal AppDb Db { get; set; }

        public Food_Property()
        {
        }

        internal Food_Property(AppDb db)
        {
            Db = db;
        }

        // insert a new food property
        public async Task InsertOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "newFoodProperty";
            cmd.CommandType = CommandType.StoredProcedure;
            BindId(cmd);
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        // update an existing food property
        public async Task UpdateOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "updateFoodProperty";
            cmd.CommandType = CommandType.StoredProcedure;
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        // binds food name
        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@fname",
                DbType = DbType.String,
                Value = foodName,
            });
        }

        // binds parameters
        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@foodProperty",
                DbType = DbType.String,
                Value = foodProperty,
            });
        }

    }
}