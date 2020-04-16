// represents a single search's criteria

using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class Search_Criteria
    {
        public int searchID { get; set; }
        public int customerID { get; set; }
        public string searchCriteria { get; set; }

        internal AppDb Db { get; set; }

        public Search_Criteria()
        {
        }

        internal Search_Criteria(AppDb db)
        {
            Db = db;
        }

        // insert a new search's criteria
        public async Task InsertOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "newSearchCriteria";
            cmd.CommandType = CommandType.StoredProcedure;
            BindId(cmd);
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
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@sCrit",
                DbType = DbType.String,
                Value = searchCriteria,
            });
        }

    }
}