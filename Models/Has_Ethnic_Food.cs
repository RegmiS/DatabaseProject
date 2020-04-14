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

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `ethic_food` (`restaurantID`, `foodName`) VALUES (@restaurantID, @foodName);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `ethic_food` SET `restaurantID` = @restaurantID, `foodName` = @foodName WHERE `restaurantID` = @restaurantID";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `ethic_food` WHERE `restaurantID` = @restaurantID;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@restaurantID",
                DbType = DbType.String,
                Value = restaurantID,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@foodName",
                DbType = DbType.String,
                Value = foodName,
            });
        }

    }
}