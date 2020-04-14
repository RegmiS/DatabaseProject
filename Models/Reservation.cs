// represents a single reservation, needs insert, update, and delete for reservations with certain ids

using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;

namespace DatabaseProject
{
    public class Reservation
    {
        public int reservationID { get; set; }
        public int crDistance { get; set; }
        public string date { get; set; }
        public int restaurantID { get; set; }
        public int customerID { get; set; }

        internal AppDb Db { get; set; }

        public Reservation()
        {
        }

        internal Reservation(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `Reservation` (`crDistance`, `date`, `restaurantID`, `customerID`) VALUES (@crDistance, @date, @restaurantID, @customerID);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            reservationID = (int)cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `Reservation` SET `crDistance` = @crDistance, `date` = @date, `restaurantID` = @restaurantID, `customerID` = @customerID WHERE `reservationID` = @reservationID";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Reservation` WHERE `reservationID` = @reservationID;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@reservationID",
                DbType = DbType.Int32,
                Value = reservationID,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@crDistance",
                DbType = DbType.Int32,
                Value = crDistance,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@date",
                DbType = DbType.String,
                Value = date,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@restaurantID",
                DbType = DbType.Int32,
                Value = restaurantID,
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