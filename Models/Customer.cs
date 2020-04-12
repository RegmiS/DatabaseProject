// represents a single customer, needs insert, update, and delete for customers with certain ids

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

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `Customer` (`firstName`, `lastName`, `address`) VALUES (@firstname, @lastname, @address);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            customerID = (int) cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `Customer` SET `firstName` = @firstname, `lastName` = @lastname, `address` = @address WHERE `customerID` = @customerid";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Customer` WHERE `customerID` = @customerid;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@customerid",
                DbType = DbType.Int32,
                Value = customerID,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@firstname",
                DbType = DbType.String,
                Value = firstName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@lastname",
                DbType = DbType.String,
                Value = lastName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@address",
                DbType = DbType.String,
                Value = address,
            });
        }

    }
}