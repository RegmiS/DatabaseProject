using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class History_EntryQuery
    {
        public AppDb Db { get; }

        public History_EntryQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<History_Entry>> GetOne(int customer)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getCustomerReservationHistory";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@customer",
                DbType = DbType.Int32,
                Value = customer,
            });
            return await AllHistoryEntries(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<History_Entry>> AllHistoryEntries(DbDataReader reader)
        {
            var all_customers = new List<History_Entry>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var single_customer = new History_Entry(Db)
                    {
                        historyID = reader.GetInt32(0),
                        reservationID = reader.GetInt32(1),
                        customerID = reader.GetInt32(2),
                    };
                    all_customers.Add(single_customer);
                }
            }
            return all_customers;
        }
    }
}