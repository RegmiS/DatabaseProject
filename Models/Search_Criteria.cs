// represents a single search criteria, needs insert, update, and delete for search criterias with certain ids

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

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `Search_Criteria` (`customerID`, `searchCriteria`) VALUES (@customerID, @searchCriteria);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            searchID = (int)cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `Search_Criteria` SET `customerID` = @customerID WHERE `SearchID` = @Searchid";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Search_Criteria` WHERE `SearchID` = @Searchid;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Searchid",
                DbType = DbType.Int32,
                Value = searchID,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@customerID",
                DbType = DbType.Int32,
                Value = customerID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@searchCriteria",
                DbType = DbType.String,
                Value = customerID,
            });
        }

    }
}