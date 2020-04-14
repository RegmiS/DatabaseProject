// represents a single history entry, needs insert, update, and delete for history entrys with certain ids

using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;

namespace DatabaseProject
{
    public class History_Entry
    {
        public int historyID { get; set; }
        public int reservationID { get; set; }
        public int customerID { get; set; }

        internal AppDb Db { get; set; }

        public History_Entry()
        {
        }

        internal History_Entry(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `History_Entry` (`reservationID`, `customerID`) VALUES (@reservationID, @customerID);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            historyID = (int)cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `History_Entry` SET `reservationID` = @reservationID, `customerID` = @customerID WHERE `historyID` = @historyID";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `History_Entry` WHERE `historyID` = @historyID;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@historyID",
                DbType = DbType.Int32,
                Value = historyID,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@reservationID",
                DbType = DbType.Int32,
                Value = reservationID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@customerID",
                DbType = DbType.Int32,
                Value = customerID,
            });
        }

    }
}