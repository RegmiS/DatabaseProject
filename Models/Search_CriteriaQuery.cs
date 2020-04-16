// search_criteria queries

using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class Search_CriteriaQuery
    {
        public AppDb Db { get; }

        public Search_CriteriaQuery(AppDb db)
        {
            Db = db;
        }

        // retreive a specific search's criteria
        public async Task<Search_Criteria> GetOne(int sid)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getSearchCriteria";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@sid",
                DbType = DbType.Int32,
                Value = sid,
            });
            var result = await AllSearch_Criterias(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        // looks through all search criterias
        private async Task<List<Search_Criteria>> AllSearch_Criterias(DbDataReader reader)
        {
            var all_searchs = new List<Search_Criteria>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var single_search = new Search_Criteria(Db)
                    {
                        searchID = reader.GetInt32(0),
                        customerID = reader.GetInt32(1),
                        searchCriteria = reader.GetString(2),
                    };
                    all_searchs.Add(single_search);
                }
            }
            return all_searchs;
        }
    }
}