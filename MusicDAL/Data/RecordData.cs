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

namespace MusicDAL.Data
{
    public class _td
    {
        #region " Methods "

        /// <summary>
        /// Get all Records from Record table
        /// </summary>
        /// <returns>A List of Record objects</returns>
        public static List<Record> GetRecords()
        {
            using (var cn = new SqlConnection(AppSettings.Instance.ConnectString))
            {
                var sql = "dbo.adm_RecordSelect";
                var Records = cn.Query<Record>(sql, commandType: CommandType.StoredProcedure).ToList();
                return Records;
            }
        }

        public static List<Record> DeserialiseRecords(List<Record> records)
        {
            records = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Record>>(File.ReadAllText("records.json"))!;
            return records;
        }

        public static void BulkInsert(List<Record> records)
        {
            // Create a DataTable to hold the data to be inserted
            DataTable table = new DataTable();
            table.Columns.Add("RecordId", typeof(int));
            table.Columns.Add("ArtistId", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("SubTitle", typeof(string));
            table.Columns.Add("Field", typeof(string));
            table.Columns.Add("Recorded", typeof(int));
            table.Columns.Add("Discs", typeof(int));
            table.Columns.Add("CoverName", typeof(string));
            table.Columns.Add("Review", typeof(string));
            table.Columns.Add("Folder", typeof(string));
            table.Columns.Add("Length", typeof(string));

            // Add the rows from the List<Record> to the DataTable
            foreach (Record record in records)
            {
                DataRow row = table.NewRow();
                row["RecordId"] = record.RecordId;
                row["ArtistId"] = record.ArtistId;
                row["Name"] = record.Name;
                row["SubTitle"] = record.SubTitle;
                row["Field"] = record.Field;
                row["Recorded"] = record.Recorded;
                row["Discs"] = record.Discs;
                row["CoverName"] = record.CoverName;
                row["Review"] = record.Review;
                row["Folder"] = record.Folder;
                row["Length"] = record.Length;
                table.Rows.Add(row);
            }

            // Open a connection to the database and create a SqlBulkCopy object
            using (SqlConnection cn = new SqlConnection(AppSettings.Instance.ConnectString))
            {
                cn.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cn))
                {
                    // Set the destination table and mapping of columns
                    bulkCopy.DestinationTableName = "dbo.Record";
                    bulkCopy.ColumnMappings.Add("RecordId", "RecordId");
                    bulkCopy.ColumnMappings.Add("ArtistId", "ArtistId");
                    bulkCopy.ColumnMappings.Add("Name", "Name");
                    bulkCopy.ColumnMappings.Add("SubTitle", "SubTitle");
                    bulkCopy.ColumnMappings.Add("Field", "Field");
                    bulkCopy.ColumnMappings.Add("Recorded", "Recorded");
                    bulkCopy.ColumnMappings.Add("Discs", "Discs");
                    bulkCopy.ColumnMappings.Add("CoverName", "CoverName");
                    bulkCopy.ColumnMappings.Add("Review", "Review");
                    bulkCopy.ColumnMappings.Add("Folder", "Folder");
                    bulkCopy.ColumnMappings.Add("Length", "Length");

                    // Perform the bulk insert
                    bulkCopy.WriteToServer(table);
                }
            }
        }

        #endregion
    }
}
