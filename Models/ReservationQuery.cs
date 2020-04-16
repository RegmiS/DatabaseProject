// reservation queries

using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;

namespace DatabaseProject
{
    public class ReservationQuery
    {
        public AppDb Db { get; }

        public ReservationQuery(AppDb db)
        {
            Db = db;
        }

        // retrieves a specific reservation
        public async Task<Reservation> GetOneReservation(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getReservationInformation";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await AllReservations(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        // retreives all reservations for a specific restaurant
        public async Task<List<Reservation>> GetAll(int rid)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getRestaurantReservations";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@rid",
                DbType = DbType.Int32,
                Value = rid,
            });
            return await AllReservations(await cmd.ExecuteReaderAsync());
        }

        // looks through all reservations
        private async Task<List<Reservation>> AllReservations(DbDataReader reader)
        {
            var all_Reservations = new List<Reservation>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var single_Reservation = new Reservation(Db)
                    {
                        reservationID = reader.GetInt32(0),
                        crDistance = reader.GetInt32(1),
                        date = reader.GetDateTime(2).ToString("MM/dd/yyyy"),
                        restaurantID = reader.GetInt32(3),
                        customerID = reader.GetInt32(4),
                    };
                    all_Reservations.Add(single_Reservation);
                }
            }
            return all_Reservations;
        }
    }
}