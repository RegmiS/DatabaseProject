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

        public async Task<Reservation> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `reservationID`, `crDistance`, `date`, `restaurantID`, `customerID` FROM `Reservation` WHERE `reservationID` = @reservationID";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@reservationID",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Reservation>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `reservationID`, `crDistance`, `date`, `restaurantID`, `customerID` FROM `Reservation` ORDER BY `reservationID`";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Reservation`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Reservation>> ReadAllAsync(DbDataReader reader)
        {
            var all_customers = new List<Reservation>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var single_customer = new Reservation(Db)
                    {
                        reservationID = reader.GetInt32(0),
                        crDistance = reader.GetInt32(1),
                        date = reader.GetDateTime(2).ToString("MM/dd/yyyy"),
                        restaurantID = reader.GetInt32(3),
                        customerID = reader.GetInt32(4),
                    };
                    all_customers.Add(single_customer);
                }
            }
            return all_customers;
        }
    }
}