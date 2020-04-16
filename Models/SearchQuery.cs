// search queries

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

        // retrieves a specific search
        public async Task<Search> GetOneSearch(int sid)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getSearchResult";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@sid",
                DbType = DbType.Int32,
                Value = sid,
            });
            var result = await AllSearchs(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        // looks through all searches
        private async Task<List<Search>> AllSearchs(DbDataReader reader)
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