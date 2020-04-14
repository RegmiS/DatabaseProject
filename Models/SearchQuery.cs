using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class SearchQuery
    {
        public AppDb Db { get; }

        public SearchQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Search> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `searchID`, `customerID` FROM `Search` WHERE `searchID` = @searchID";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@searchID",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Search>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `searchID`, `customerID` FROM `Search` ORDER BY `searchID`";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Search`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Search>> ReadAllAsync(DbDataReader reader)
        {
            var all_searchs = new List<Search>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var single_search = new Search(Db)
                    {
                        searchID = reader.GetInt32(0),
                        customerID = reader.GetInt32(1),
                    };
                    all_searchs.Add(single_search);
                }
            }
            return all_searchs;
        }
    }
}