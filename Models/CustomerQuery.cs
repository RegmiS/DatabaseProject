using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class CustomerQuery
    {
        public AppDb Db { get; }

        public CustomerQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Customer> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `customerID`, `firstName`, `lastName`, `address` FROM `Customer` WHERE `customerID` = @customerid";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@customerid",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Customer>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `customerID`, `firstName`, `lastName`, `address` FROM `Customer` ORDER BY `customerID`";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Customer`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Customer>> ReadAllAsync(DbDataReader reader)
        {
            var all_customers = new List<Customer>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var single_customer = new Customer(Db)
                    {
                        customerID = reader.GetInt32(0),
                        lastName = reader.GetString(1),
                        firstName = reader.GetString(2),
                        address = reader.GetString(3),
                    };
                    all_customers.Add(single_customer);
                }
            }
            return all_customers;
        }
    }
}