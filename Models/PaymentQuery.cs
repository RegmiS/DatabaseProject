using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class PaymentQuery
    {
        public AppDb Db { get; }

        public PaymentQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Payment> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `paymentID`, `reservationID`, `paymentMethod`, `paymentAmount` FROM `Payment` WHERE `paymentID` = @paymentID";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@paymentID",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Payment>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `paymentID`, `reservationID`, `paymentMethod`, `paymentAmount` FROM `Payment` ORDER BY `paymentID`";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Payment`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Payment>> ReadAllAsync(DbDataReader reader)
        {
            var all_customers = new List<Payment>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var single_customer = new Payment(Db)
                    {
                        paymentID = reader.GetInt32(0),
                        reservationID = reader.GetInt32(1),
                        paymentMethod = reader.GetString(2),
                        paymentAmount = reader.GetFloat(3),
                    };
                    all_customers.Add(single_customer);
                }
            }
            return all_customers;
        }
    }
}