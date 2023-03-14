using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDAL.Models
{
    public class Disk
    {
        #region " Properties "

        /// <summary>
        /// Gets or sets the disc id.
        /// </summary>
        public int DiscId { get; set; }

        /// <summary>
        /// Gets or sets the record id.
        /// </summary>
        public int RecordId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the sub title.
        /// </summary>
        public string? SubTitle { get; set; }

        /// <summary>
        /// Gets or sets the disc number.
        /// </summary>
        public int DiscNumber { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public string? Length { get; set; }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        public string? Folder { get; set; }

        #endregion
    }
}
