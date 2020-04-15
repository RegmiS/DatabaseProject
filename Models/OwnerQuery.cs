using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class OwnerQuery
    {
        public AppDb Db { get; }

        public OwnerQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Owner> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `ownerID`, `firstName`, `lastName` FROM `Owner` WHERE `ownerID` = @ownerid";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ownerid",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Owner>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `ownerID`, `firstName`, `lastName` FROM `Owner` ORDER BY `ownerID`";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `Owner`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Owner>> ReadAllAsync(DbDataReader reader)
        {
            var all_owners = new List<Owner>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var single_owner = new Owner(Db)
                    {
                        ownerID = reader.GetInt32(0),
                        firstName = reader.GetString(1),
                        lastName = reader.GetString(2),
                    };
                    all_owners.Add(single_owner);
                }
            }
            return all_owners;
        }
    }
}