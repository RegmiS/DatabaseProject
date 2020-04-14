using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;

namespace DatabaseProject
{
    public class ReviewQuery
    {
        public AppDb Db { get; }

        public ReviewQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Review> FindOneAsync(int reviewNumber)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `reviewNumber`, `comment`, `rating`, `restaurantID`, `customerID`, `date` FROM `Review` WHERE `reviewNumber` = @reviewNumber";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@reviewNumber",
                DbType = DbType.Int32,
                Value = reviewNumber,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Review>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `reviewNumber`, `comment`, `rating`, `restaurantID`, `customerID`, `date` FROM `Review` ORDER BY `reviewNumber`";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Review`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Review>> ReadAllAsync(DbDataReader reader)
        {
            var all_customers = new List<Review>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var single_customer = new Review(Db)
                    {
                        reviewNumber = reader.GetInt32(0),
                        comment = reader.GetString(1),
                        rating = reader.GetInt32(2),
                        restaurantID = reader.GetInt32(3),
                        customerID = reader.GetInt32(4),
                        date = reader.GetDateTime(5).ToString("MM/dd/yyyy"),
                    };
                    all_customers.Add(single_customer);
                }
            }
            return all_customers;
        }
    }
}