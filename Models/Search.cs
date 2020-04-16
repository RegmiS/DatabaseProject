// represents a single search

using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class Search
    {
        public int searchID { get; set; }
        public int customerID { get; set; }

        internal AppDb Db { get; set; }

        public Search()
        {
        }

        internal Search(AppDb db)
        {
            Db = db;
        }

        // insert a new search
        public async Task InsertOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "newSearch";
            cmd.CommandType = CommandType.StoredProcedure;
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            searchID = (int)cmd.LastInsertedId;
        }

        // binds search ID
        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@sid",
                DbType = DbType.Int32,
                Value = searchID,
            });
        }

        // binds parameters
        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@cid",
                DbType = DbType.Int32,
                Value = customerID,
            });
        }

    }
}