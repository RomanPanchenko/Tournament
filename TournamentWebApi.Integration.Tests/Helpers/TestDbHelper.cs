using Npgsql;
using System.Configuration;
using System.IO;

namespace TournamentWebApi.Integration.Tests.Helpers
{
    public class TestDbHelper
    {
        public void CreateDbStructure()
        {
            string connStr = ConfigurationManager.ConnectionStrings["XNU-Test"].ConnectionString;
            var conn = new NpgsqlConnection(connStr);
            conn.Open();
            var createdbCmd = new NpgsqlCommand(@"DROP DATABASE IF EXISTS TournamentTest;", conn);
            createdbCmd.ExecuteNonQuery();
            conn.Close();
            conn.Open();
            createdbCmd = new NpgsqlCommand(@"CREATE DATABASE TournamentTest WITH OWNER = postgres ENCODING = 'UTF8' CONNECTION LIMIT = -1;", conn);
            createdbCmd.ExecuteNonQuery();
            conn.Close();

            connStr = ConfigurationManager.ConnectionStrings["XNU"].ConnectionString;
            conn = new NpgsqlConnection(connStr);
            conn.Open();
            string query = File.ReadAllText(@"..\..\..\TournamentWebApi.DB\Tables\Players.sql");
            createdbCmd = new NpgsqlCommand(query, conn);
            createdbCmd.ExecuteNonQuery();
            query = File.ReadAllText(@"..\..\..\TournamentWebApi.DB\Tables\Matches.sql");
            createdbCmd = new NpgsqlCommand(query, conn);
            createdbCmd.ExecuteNonQuery();
            conn.Close();
        }

        public void SeedDbWithTestData()
        {
            string connStr = ConfigurationManager.ConnectionStrings["XNU"].ConnectionString;
            var conn = new NpgsqlConnection(connStr);
            conn.Open();
            string query = File.ReadAllText(@"..\..\..\TournamentWebApi.DB\Seed\SeedData.sql");
            var createdbCmd = new NpgsqlCommand(query, conn);
            createdbCmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
