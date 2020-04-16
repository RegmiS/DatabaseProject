// owner queries

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

        // retrieves a specific owner
        public async Task<Owner> GetOne(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getOneOwner";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await AllOwners(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        // retrieves all owners
        public async Task<List<Owner>> GetAll()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getOwners";
            cmd.CommandType = CommandType.StoredProcedure;
            return await AllOwners(await cmd.ExecuteReaderAsync());
        }

        // looks through all owners
        private async Task<List<Owner>> AllOwners(DbDataReader reader)
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