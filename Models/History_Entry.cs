// represents a single history entry

using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;

namespace DatabaseProject
{
    public class History_Entry
    {
        public int historyID { get; set; }
        public int reservationID { get; set; }
        public int customerID { get; set; }

        internal AppDb Db { get; set; }

        public History_Entry()
        {
        }

        internal History_Entry(AppDb db)
        {
            Db = db;
        }

        // insert a new history entry for a customer
        public async Task InsertOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "newHistoryEntry";
            cmd.CommandType = CommandType.StoredProcedure;
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            historyID = (int)cmd.LastInsertedId;
        }

        // bind history ID
        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@history",
                DbType = DbType.Int32,
                Value = historyID,
            });
        }

        // bind parameters
        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@reservation",
                DbType = DbType.Int32,
                Value = reservationID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@customer",
                DbType = DbType.Int32,
                Value = customerID,
            });
        }

    }
}