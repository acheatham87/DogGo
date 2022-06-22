using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;

namespace DogGo.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IConfiguration _config;

        // The consturctor accepts and Iconfiguration object as a parameter.  This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file connection strings.
        public WalkRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walk> GetWalksByWalkerId(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT w.Date, w.Duration, o.Name
                    FROM Walks w
                    LEFT JOIN Dog d ON w.DogId = d.Id
                    LEFT JOIN Owner o ON d.OwnerId = o.Id
                    WHERE w.WalkerId = @walkerId;";

                    cmd.Parameters.AddWithValue("@walkerId", walkerId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walk> walks = new List<Walk>();

                        while (reader.Read())
                        {
                            Walk walk = new Walk
                            {
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                Client = new Owner
                                {
                                    Name = reader.GetString(reader.GetOrdinal("Name"))
                                }
                            };

                            walks.Add(walk);
                        }

                        return walks;
                    }
                }
            }
        }
    }
}
