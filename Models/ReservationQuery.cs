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

        public async Task<List<Reservation>> GetAll()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getAllReservation";
            cmd.CommandType = CommandType.StoredProcedure;
            return await AllReservations(await cmd.ExecuteReaderAsync());
        }

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