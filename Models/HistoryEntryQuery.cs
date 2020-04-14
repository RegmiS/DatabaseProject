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

        public async Task<History_Entry> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `historyID`, `reservationID`, `customerID` FROM `History_Entry` WHERE `historyID` = @historyID";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@historyID",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<History_Entry>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `historyID`, `reservationID`, `customerID` FROM `History_Entry` ORDER BY `historyID`";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `History_Entry`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<History_Entry>> ReadAllAsync(DbDataReader reader)
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