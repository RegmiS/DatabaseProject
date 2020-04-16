// customer queries

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

        // retrieves a specific customer
        public async Task<Customer> GetOneCustomer(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getOneCustomer";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await AllCustomers(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        // retrieves all customers
        public async Task<List<Customer>> GetAll()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getCustomers";
            cmd.CommandType = CommandType.StoredProcedure;
            return await AllCustomers(await cmd.ExecuteReaderAsync());
        }

        // looks through all customers
        private async Task<List<Customer>> AllCustomers(DbDataReader reader)
        {
            var all_customers = new List<Customer>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var single_customer = new Customer(Db)
                    {
                        customerID = reader.GetInt32(0),
                        firstName = reader.GetString(1),
                        lastName = reader.GetString(2),
                        address = reader.GetString(3),
                    };
                    all_customers.Add(single_customer);
                }
            }
            return all_customers;
        }
    }
}