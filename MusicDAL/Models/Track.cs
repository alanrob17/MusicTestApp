using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDAL.Models
{
    public class Track
    {
        #region " Properties "

        /// <summary>
        /// Gets or sets the track Id.
        /// </summary>
        public int TrackId { get; set; }

        /// <summary>
        /// Gets or sets the disc Id.
        /// </summary>
        public int DiscId { get; set; }

        /// <summary>
        /// Gets or sets the disc number.
        /// </summary>
        public int? DiscNumber { get; set; }

        /// <summary>
        /// Gets or sets the track name from filename.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the title from the tags.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the year recorded.
        /// </summary>
        public int? Recorded { get; set; }

        /// <summary>
        /// Gets or sets the length of a track in minutes and seconds.
        /// </summary>
        public string? Length { get; set; }

        /// <summary>
        /// Gets or sets the duration of a track in timespan.
        /// </summary>
        public TimeSpan? Duration { get; set; }

        /// <summary>
        /// Gets or sets the bits.
        /// </summary>
        public int? Bits { get; set; }

        /// <summary>
        /// Gets or sets the bit rate.
        /// </summary>
        public int? BitRate { get; set; }

        /// <summary>
        /// Gets or sets the audio sample rate.
        /// </summary>
        public int? AudioSampleRate { get; set; }

        /// <summary>
        /// Gets or sets the audio channels.
        /// </summary>
        public int? AudioChannels { get; set; }

        /// <summary>
        /// Gets or sets the type of the music file.
        /// </summary>
        public string? Media { get; set; }

        /// <summary>
        /// Gets or sets the album name.
        /// </summary>
        public string? Album { get; set; }

        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        public string? Artist { get; set; }

        /// <summary>
        /// Gets or sets the field (Genre).
        /// </summary>
        public string? Field { get; set; }

        /// <summary>
        /// Gets or sets the track number.
        /// </summary>
        public int? Number { get; set; }

        /// <summary>
        /// Gets or sets the full directory and name.
        /// </summary>
        public string? Folder { get; set; }

        #endregion
    }
}
