using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Eagle.Resources.ResourceBuilder
{
    public class DbResourceProvider : BaseResourceProvider
    {
        // Database connection string        
        private static string _connectionString;
        public DbResourceProvider()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
        public DbResourceProvider(string connection)
        {
            _connectionString = connection;
        }
        protected override IList<ResourceEntry> ReadResources()
        {
            var resources = new List<ResourceEntry>();
            const string sql = "select Culture, Name, Value from dbo.Resources;";
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(sql, con);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        resources.Add(new ResourceEntry
                        {
                            Name = reader["Name"].ToString(),
                            Value = reader["Value"].ToString(),
                            Culture = reader["Culture"].ToString()
                        });
                    }
                    if (!reader.HasRows) throw new Exception("No resources were found");
                }
            }
            return resources;

        }
        protected override ResourceEntry ReadResource(string name, string culture)
        {
            ResourceEntry resource = null;
            const string sql = "select Culture, Name, Value from dbo.Resources where culture = @culture and name = @name;";
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@culture", culture);
                cmd.Parameters.AddWithValue("@name", name);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        resource = new ResourceEntry
                        {
                            Name = reader["Name"].ToString(),
                            Value = reader["Value"].ToString(),
                            Culture = reader["Culture"].ToString()
                        };
                    }
                    if (!reader.HasRows) throw new Exception($"Resource {name} for culture {culture} was not found");
                }
            }
            return resource;

        }


    }
}
