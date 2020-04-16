// represents a single customer

using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class Customer{
        public int customerID{get;set;}
        public string firstName{get; set;}
        public string lastName{get; set;}
        public string address{get;set;}

        internal AppDb Db { get; set; }

        public Customer()
        {
        }

        internal Customer(AppDb db)
        {
            Db = db;
        }

        // insert a new customer
        public async Task InsertOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "newCustomer";
            cmd.CommandType = CommandType.StoredProcedure;
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            customerID = (int) cmd.LastInsertedId;
        }

        // update an existing customer
        public async Task UpdateOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "updateCustomerInformation";
            cmd.CommandType = CommandType.StoredProcedure;
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        // binds customer ID
        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = customerID,
            });
        }

        // binds parameters
        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@first",
                DbType = DbType.String,
                Value = firstName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@last",
                DbType = DbType.String,
                Value = lastName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@addr",
                DbType = DbType.String,
                Value = address,
            });
        }

    }
}