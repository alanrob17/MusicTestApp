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
    public class DiskData
    {
        #region " Methods "

        /// <summary>
        /// Get all discs.
        /// NOTE: I can't run this in Dapper because of a TimeSpan issue.
        /// </summary>
        /// <returns>The <see cref="List"/>discs.</returns>
        public static List<Disk> GetDiscs()
        {
            var dt = new DataTable();

            using (var cn = new SqlConnection(AppSettings.Instance.ConnectString))
            {
                var sql = "dbo.adm_DiscSelect";
                var cmd = new SqlCommand(sql, cn) { CommandType = CommandType.StoredProcedure };

                var da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            var query = from dr in dt.AsEnumerable()
                        select new Disk
                        {
                            DiscId = Convert.ToInt32(dr["DiscId"]),
                            RecordId = Convert.ToInt32(dr["RecordId"]),
                            Name = dr["Name"].ToString(),
                            SubTitle = dr["SubTitle"].ToString(),
                            DiscNumber = Convert.ToInt32(dr["DiscNumber"]),
                            Length = dr["Length"].ToString(),
                            Duration = TimeSpan.FromTicks((long)dr["Duration"]),
                            Folder = dr["Folder"].ToString()
                        };

            return query.ToList();
        }

        public static List<Disk> DeserialiseDiscs(List<Disk> discs)
        {
            discs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Disk>>(File.ReadAllText("discs.json"))!;
            return discs;
        }

        public static void BulkInsert(List<Disk> discs)
        {
            using (SqlConnection cn = new SqlConnection(AppSettings.Instance.ConnectString))
            {
                cn.Open();
                using (SqlTransaction transaction = cn.BeginTransaction())
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cn, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.BatchSize = 1000;
                        bulkCopy.DestinationTableName = "dbo.Disc";
                        bulkCopy.ColumnMappings.Add("DiscId", "DiscId");
                        bulkCopy.ColumnMappings.Add("RecordId", "RecordId");
                        bulkCopy.ColumnMappings.Add("Name", "Name");
                        bulkCopy.ColumnMappings.Add("SubTitle", "SubTitle");
                        bulkCopy.ColumnMappings.Add("DiscNumber", "DiscNumber");
                        bulkCopy.ColumnMappings.Add("Length", "Length");
                        bulkCopy.ColumnMappings.Add("Duration", "Duration");
                        bulkCopy.ColumnMappings.Add("Folder", "Folder");
                        try
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("DiscId", typeof(int));
                            dataTable.Columns.Add("RecordId", typeof(int));
                            dataTable.Columns.Add("Name", typeof(string));
                            dataTable.Columns.Add("SubTitle", typeof(string));
                            dataTable.Columns.Add("DiscNumber", typeof(int));
                            dataTable.Columns.Add("Length", typeof(string));
                            dataTable.Columns.Add("Duration", typeof(TimeSpan));
                            dataTable.Columns.Add("Folder", typeof(string));
                            foreach (Disk disc in discs)
                            {
                                DataRow row = dataTable.NewRow();
                                row["DiscId"] = disc.DiscId;
                                row["RecordId"] = disc.RecordId;
                                row["Name"] = disc.Name;
                                row["SubTitle"] = disc.SubTitle;
                                row["DiscNumber"] = disc.DiscNumber;
                                row["Length"] = disc.Length;
                                row["Duration"] = disc.Duration;
                                row["Folder"] = disc.Folder;
                                dataTable.Rows.Add(row);
                            }
                            bulkCopy.WriteToServer(dataTable);
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
