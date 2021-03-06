// represents a single owner, needs insert, update, and delete for owners with certain ids

using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class Owner
    {
        public int ownerID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        internal AppDb Db { get; set; }

        public Owner()
        {
        }

        internal Owner(AppDb db)
        {
            Db = db;
        }

        public async Task InsertOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "newOwner";
            cmd.CommandType = CommandType.StoredProcedure;
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            ownerID = (int)cmd.LastInsertedId;
        }

        public async Task UpdateOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "updateOwnerInformation";
            cmd.CommandType = CommandType.StoredProcedure;
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = ownerID,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@first",
                DbType = DbType.String,
                Value = firstName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@last",
                DbType = DbType.String,
                Value = lastName,
            });
        }

    }
}