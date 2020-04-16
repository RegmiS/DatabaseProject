// represents a single reservation

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

        // insert a new reservation
        public async Task InsertOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "newReservation";
            cmd.CommandType = CommandType.StoredProcedure;
            BindId(cmd);
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@edate",
                DbType = DbType.Date,
                Value = date,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rid",
                DbType = DbType.Int32,
                Value = restaurantID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@cid",
                DbType = DbType.Int32,
                Value = customerID,
            });
            await cmd.ExecuteNonQueryAsync();
            reservationID = (int)cmd.LastInsertedId;
        }

        // binds reservation ID
        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = reservationID,
            });
        }

        // binds parameters
        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@crdist",
                DbType = DbType.Int32,
                Value = crDistance,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@edate",
                DbType = DbType.Date,
                Value = date,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rid",
                DbType = DbType.Int32,
                Value = restaurantID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@cid",
                DbType = DbType.Int32,
                Value = customerID,
            });
        }

    }
}