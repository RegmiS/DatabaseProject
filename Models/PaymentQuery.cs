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

        public async Task<Payment> GetOne(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getPaymentInformation";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await AllPayments(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        private async Task<List<Payment>> AllPayments(DbDataReader reader)
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