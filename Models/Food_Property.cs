// represents a single food property, needs insert, update, and delete for foodss with certain names

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

        public async Task InsertOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "newFoodProperty";
            cmd.CommandType = CommandType.StoredProcedure;
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "updateFoodInformation";
            cmd.CommandType = CommandType.StoredProcedure;
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@foodName",
                DbType = DbType.String,
                Value = foodName,
            });
        }

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