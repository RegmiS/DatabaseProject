// represents a single ethnic food

using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class Ethnic_Food
    {
        public string name { get; set; }
        public string ethnicity { get; set; }

        internal AppDb Db { get; set; }

        public Ethnic_Food()
        {
        }

        internal Ethnic_Food(AppDb db)
        {
            Db = db;
        }

        // insert new ethnic food
        public async Task InsertOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "newEthnicFood";
            cmd.CommandType = CommandType.StoredProcedure;
            BindId(cmd);
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        // update an existing ethnic food
        public async Task UpdateOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "updateFoodEthnicity";
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
                Value = name,
            });
        }

        // binds parameters
        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@eEthnicity",
                DbType = DbType.String,
                Value = ethnicity,
            });
        }

    }
}