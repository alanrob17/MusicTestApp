using Dapper;
using FastMember;
using MusicDAL.Components;
using MusicDAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDAL.Data
{
    public class ArtistData
    {
        #region " Methods "

        /// <summary>
        /// Get all Artists from Artist table
        /// </summary>
        /// <returns>A List of Artist objects</returns>
        public static List<Artist> GetArtists()
        {
            using (var cn = new SqlConnection(AppSettings.Instance.ConnectString))
            {
                var sql = "dbo.adm_ArtistSelect";
                var artists = cn.Query<Artist>(sql, commandType: CommandType.StoredProcedure).ToList();
                return artists;
            }
        }

        public static List<Artist> DeserialiseArtists(List<Artist> artists)
        {
            artists = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Artist>>(File.ReadAllText("artists.json"))!;
            return artists;
        }

        public static void BulkInsert(List<Artist> artists)
        {
            using (SqlConnection cn = new SqlConnection(AppSettings.Instance.ConnectString))
            {
                cn.Open();

                // Create a SqlBulkCopy object
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cn))
                {
                    bulkCopy.DestinationTableName = "Artist";

                    // Use FastMember to create an ObjectReader from the list of Atist objects
                    // Install-Package FastMember
                    using (ObjectReader reader = ObjectReader.Create(artists))
                    {
                        // Set up the column mappings between the ObjectReader and the SQL Server table
                        bulkCopy.ColumnMappings.Add("ArtistId", "ArtistId");
                        bulkCopy.ColumnMappings.Add("FirstName", "FirstName");
                        bulkCopy.ColumnMappings.Add("LastName", "LastName");
                        bulkCopy.ColumnMappings.Add("Name", "Name");
                        bulkCopy.ColumnMappings.Add("Biography", "Biography");
                        bulkCopy.ColumnMappings.Add("Folder", "Folder");
                        bulkCopy.ColumnMappings.Add("RecordArtistId", "RecordArtistId");
                        // Write the data to the SQL Server table
                        bulkCopy.WriteToServer(reader);
                    }
                }
            }
        }

        #endregion
    }
}
