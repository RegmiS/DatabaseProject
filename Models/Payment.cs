// represents a single payment

using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class Payment
    {
        public int paymentID { get; set; }
        public int reservationID { get; set; }
        public string paymentMethod { get; set; }
        public float paymentAmount { get; set; }

        internal AppDb Db { get; set; }

        public Payment()
        {
        }

        internal Payment(AppDb db)
        {
            Db = db;
        }

        // insert a new payment 
        public async Task InsertOne()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "newPaymentInformation";
            cmd.CommandType = CommandType.StoredProcedure;
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            paymentID = (int)cmd.LastInsertedId;
        }

        // bind payment ID
        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = paymentID,
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
                ParameterName = "@method",
                DbType = DbType.String,
                Value = paymentMethod,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@amount",
                DbType = DbType.Single,
                Value = paymentAmount,
            });
        }

    }
}