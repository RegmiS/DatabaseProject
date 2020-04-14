// represents a single payment, needs insert, update, and delete for payments with certain ids

using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class Payment
    {
        public int paymentID { get; set; }
        public int reservationID { get; set; }
        public string paymentMethod { get; set; }
        public float paymentAmount { get; set; }

        internal AppDb Db { get; set; }

        public Payment()
        {
        }

        internal Payment(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `Payment` (`reservationID`, `paymentMethod`, `paymentAmount`) VALUES (@reservationID, @paymentMethod, @paymentAmount);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            paymentID = (int)cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `Payment` SET `reservationID` = @reservationID, `paymentMethod` = @paymentMethod, `paymentAmount` = @paymentAmount WHERE `paymentID` = @paymentID";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Payment` WHERE `paymentID` = @paymentID;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@paymentID",
                DbType = DbType.Int32,
                Value = paymentID,
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
                ParameterName = "@paymentMethod",
                DbType = DbType.String,
                Value = paymentMethod,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@paymentAmount",
                DbType = DbType.Single,
                Value = paymentMethod,
            });
        }

    }
}