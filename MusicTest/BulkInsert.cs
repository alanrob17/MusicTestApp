using MusicDAL.Data;
using MusicDAL.Models;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using _ad=MusicDAL.Data.ArtistData;
using _rd=MusicDAL.Data._td;
using _dd=MusicDAL.Data.DiskData;
using _td=MusicDAL.Data.TrackData;

namespace MusicTest
{
    public class BulkInsert
    {
        public static void Main(string[] args)
        {
            //// First section
            // Use this to save the data to disc in JSON format.
            // SerialiseData();

            //// Second section
            // Read the data from disc.
            List<Artist> artists = new();
            artists = _ad.DeserialiseArtists(artists);

            List<Record> records = new();
            records = _rd.DeserialiseRecords(records);


            List<Disk> discs = new();
            discs = _dd.DeserialiseDiscs(discs);

            // discs = discs.OrderByDescending(d => d.Duration).ToList();

            // I found TimeSpans larger than 1 day. They won't go back in the db.
            //foreach (Disk disc in discs)
            //{
            //    if (disc.Duration > TimeSpan.FromDays(1)) // compare with TimeSpan representing 1 day
            //    {
            //        disc.Duration -= TimeSpan.FromDays(1);
            //        Console.WriteLine($"{disc.DiscId} - {disc.Duration} - {disc.Name}");
            //    }
            //}

            List<Track> tracks = new();
            tracks = _td.DeserialiseTracks(tracks);

            // CheckTrackFieldLengths(tracks);
           
            //// Third section
            // _ad.BulkInsert(artists);
            // _rd.BulkInsert(records);
            _dd.BulkInsert(discs);
            // _td.BulkInsert(tracks);

            // I have the Bulk inserts working. Now I need to clean and update the data using routines in MusicBDAPI

        }

        // I need to check the length of each field in the Track table.
        private static void CheckTrackFieldLengths(List<Track> tracks)
        {
                var name = 0;
                var title = 0;
                var length = 0;
                var media = 0;
                var album = 0;
                var folder = 0;
                var artist = 0;
                var field = 0;

            foreach (Track track in tracks)
            {
                if (!string.IsNullOrEmpty(track.Name) && track.Name.Length > name)
                {
                    name = track.Name.Length;
                }

                if (!string.IsNullOrEmpty(track.Title) && track.Title.Length > title)
                {
                    title = track.Title.Length;
                }

                if (!string.IsNullOrEmpty(track.Length) && track.Length.Length > length)
                {
                    length = track.Length.Length;
                }

                if (!string.IsNullOrEmpty(track.Media) && track.Media.Length > media)
                {
                    media = track.Media.Length;
                }

                if (!string.IsNullOrEmpty(track.Album) && track.Album.Length > album)
                {
                    album = track.Album.Length;
                }

                if (!string.IsNullOrEmpty(track.Artist) && track.Artist.Length > artist)
                {
                    artist = track.Artist.Length;
                }

                if (!string.IsNullOrEmpty(track.Field) && track.Field.Length > field)
                {
                    field = track.Field.Length;
                }

                if (!string.IsNullOrEmpty(track.Folder) && track.Folder.Length > folder)
                {
                    folder = track.Folder.Length;
                }
            }

            Console.WriteLine($"Name = {name}.");
            Console.WriteLine($"Title = {title}.");
            Console.WriteLine($"Length = {length}.");
            Console.WriteLine($"Media = {media}.");
            Console.WriteLine($"Album = {album}.");
            Console.WriteLine($"Folder = {folder}.");
            Console.WriteLine($"Artist = {artist}.");
            Console.WriteLine($"Field = {field}.");
        }

        private static void SerialiseData()
        {
            List<Artist> artists = new();
            artists = ArtistData.GetArtists();

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(artists);

            File.WriteAllText("artists.json", json);

            List<Record> records = new();
            records = _rd.GetRecords();

            json = Newtonsoft.Json.JsonConvert.SerializeObject(records);

            File.WriteAllText("records.json", json);

            List<Disk> discs = new();
            discs = DiskData.GetDiscs();

            json = Newtonsoft.Json.JsonConvert.SerializeObject(discs);

            File.WriteAllText("discs.json", json);

            List<Track> tracks = new();
            tracks = TrackData.GetTracks();

            json = Newtonsoft.Json.JsonConvert.SerializeObject(tracks);

            File.WriteAllText("tracks.json", json);
        }
    }
}