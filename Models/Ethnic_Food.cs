// represents a single ethnicity, needs insert, update, and delete for customers with certain ids

using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseProject
{
    public class Ethnic_Food{
        public string name{get;set;}
        public string ethnicity{get; set;}
        internal AppDb Db { get; set; }

        public Ethnic_Food()
        {
        }

        internal Ethnic_Food(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `ethic_food` (`name`, `ethnicity`) VALUES (@name, @ethnicity);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `ethic_food` SET `name` = @name, `ethnicity` = @ethnicity WHERE `name` = @name";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `ethic_food` WHERE `name` = @name;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@name",
                DbType = DbType.String,
                Value = name,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ethnicity",
                DbType = DbType.String,
                Value = ethnicity,
            });
        }

    }
}