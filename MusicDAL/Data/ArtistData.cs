using Dapper;
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
            using (var cn = new SqlConnection(AppSettings.Instance.ConnectString))
            {
                cn.Open();

                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {
                        // Create a DataTable to hold the artist data
                        var dataTable = new DataTable();
                        dataTable.Columns.Add("ArtistId", typeof(int));
                        dataTable.Columns.Add("FirstName", typeof(string));
                        dataTable.Columns.Add("LastName", typeof(string));
                        dataTable.Columns.Add("Name", typeof(string));
                        dataTable.Columns.Add("Biography", typeof(string));
                        dataTable.Columns.Add("Folder", typeof(string));
                        dataTable.Columns.Add("RecordArtistId", typeof(int));

                        // Add each artist to the DataTable
                        foreach (var artist in artists)
                        {
                            dataTable.Rows.Add(
                                artist.ArtistId,
                                artist.FirstName,
                                artist.LastName,
                                artist.Name,
                                artist.Biography,
                                artist.Folder,
                                artist.RecordArtistId
                            );
                        }

                        // Use a SQL Server table-valued parameter to bulk insert the artists
                        var command = new SqlCommand("adm_InsertArtists", cn, transaction);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@artists", dataTable);
                        command.ExecuteNonQuery();

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Roll back the transaction if an error occurs
                        transaction.Rollback();
                        throw new Exception("Error occurred while bulk inserting artists", ex);
                    }
                }
            }
        }
        #endregion
    }
}
