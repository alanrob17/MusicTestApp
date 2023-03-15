using MusicDAL.Components;
using MusicDAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using FastMember;
using System.ComponentModel;

namespace MusicDAL.Data
{
    public class TrackData
    {
        #region " Methods "

        /// <summary>
        /// Select all tracks.
        /// </summary>
        /// <returns>The <see cref="List"/>tracks.</returns>
        public static List<Track> GetTracks()
        {
            using (var cn = new SqlConnection(AppSettings.Instance.ConnectString))
            {
                var sql = "dbo.adm_TrackSelect";
                var Tracks = cn.Query<Track>(sql, commandType: CommandType.StoredProcedure).ToList();
                return Tracks;
            }
        }

        public static List<Track> DeserialiseTracks(List<Track> tracks)
        {
            tracks = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Track>>(File.ReadAllText("tracks.json"))!;
            return tracks;
        }

        public static void BulkInsert(List<Track> tracks)
        {
            using (SqlConnection cn = new SqlConnection(AppSettings.Instance.ConnectString))
            {
                cn.Open();

                // Create a SqlBulkCopy object
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cn))
                {
                    bulkCopy.DestinationTableName = "Track";

                    // Use FastMember to create an ObjectReader from the list of Track objects
                    // Install-Package FastMember
                    using (ObjectReader reader = ObjectReader.Create(tracks))
                    {
                        // Set up the column mappings between the ObjectReader and the SQL Server table
                        bulkCopy.ColumnMappings.Add("TrackId", "TrackId");
                        bulkCopy.ColumnMappings.Add("DiscId", "DiscId");
                        bulkCopy.ColumnMappings.Add("DiscNumber", "DiscNumber");
                        bulkCopy.ColumnMappings.Add("Name", "Name");
                        bulkCopy.ColumnMappings.Add("Title", "Title");
                        bulkCopy.ColumnMappings.Add("Recorded", "Recorded");
                        bulkCopy.ColumnMappings.Add("Length", "Length");
                        bulkCopy.ColumnMappings.Add("Duration", "Duration");
                        bulkCopy.ColumnMappings.Add("Bits", "Bits");
                        bulkCopy.ColumnMappings.Add("BitRate", "BitRate");
                        bulkCopy.ColumnMappings.Add("AudioSampleRate", "AudioSampleRate");
                        bulkCopy.ColumnMappings.Add("AudioChannels", "AudioChannels");
                        bulkCopy.ColumnMappings.Add("Media", "Media");
                        bulkCopy.ColumnMappings.Add("Album", "Album");
                        bulkCopy.ColumnMappings.Add("Artist", "Artist");
                        bulkCopy.ColumnMappings.Add("Field", "Field");
                        bulkCopy.ColumnMappings.Add("Number", "Number");
                        bulkCopy.ColumnMappings.Add("Folder", "Folder");

                        // Write the data to the SQL Server table
                        bulkCopy.WriteToServer(reader);
                    }
                }
            }
        }

        #endregion
    }
}
