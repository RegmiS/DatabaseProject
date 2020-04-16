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


        public async Task InsertOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "newReservation";
            cmd.CommandType = CommandType.StoredProcedure;
            BindId(cmd);
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            reservationID = (int) cmd.LastInsertedId;
        }

        public async Task UpdateOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "updateReservationInformation";
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
                Value = reservationID,
            });
        }

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