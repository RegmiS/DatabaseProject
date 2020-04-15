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

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `Owner` (`firstName`, `lastName`) VALUES (@firstname, @lastname);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            ownerID = (int)cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `Owner` SET `firstName` = @firstname, `lastName` = @lastname WHERE `OwnerID` = @Ownerid";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Owner` WHERE `OwnerID` = @Ownerid;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Ownerid",
                DbType = DbType.Int32,
                Value = ownerID,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@firstname",
                DbType = DbType.String,
                Value = firstName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@lastname",
                DbType = DbType.String,
                Value = lastName,
            });
        }

    }
}