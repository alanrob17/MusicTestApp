using MusicDAL.Data;
using MusicDAL.Models;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using _ad = MusicDAL.Data.ArtistData;
using _rd = MusicDAL.Data.RecordData;
using _dd = MusicDAL.Data.DiskData;
using _td = MusicDAL.Data.TrackData;
using System.Text.RegularExpressions;

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
            //List<Artist> artists = new();
            //artists = _ad.DeserialiseArtists(artists);

            //List<Record> records = new();
            //records = _rd.DeserialiseRecords(records);

            //List<Disk> discs = new();
            //discs = _dd.DeserialiseDiscs(discs);

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

            //// Fix faulty names
            // CreateFaultyTrackList();

            // Dictionary<int, string> namesDictionary = ReadInFaultyTracks();

            // FixNames(namesDictionary);
            //// end.
            //// Fix faulty Title fields
            //Dictionary<int, string> titlesDictionary = ReadInFaultyTracks();

            //FixTitles(titlesDictionary);
            //// end.

            //// Find all faulty numbered tracks
            ListAllFaultyTracks();
            //// end.

            //List<Track> tracks = new();
            //tracks = _td.DeserialiseTracks(tracks);

            // CheckTrackFieldLengths(tracks);

            //// Third section
            // _ad.BulkInsert(artists);
            // _rd.BulkInsert(records);
            // _dd.BulkInsert(discs);
            // _td.BulkInsert(tracks);

            // I have the Bulk inserts working. Now I need to clean and update the data using routines in MusicBDAPI

        }

        private static void ListAllFaultyTracks()
        {
            List<Track> tracks = new List<Track>();
            tracks = _td.GetTracks();

            foreach (Track track in tracks)
            {
                var name = Path.GetFileName(track.Folder);
                Regex numberDashRegex = new Regex(@"^\d{2,3}\s-\s");
                if (!numberDashRegex.IsMatch(name))
                {
                    Console.WriteLine($"{track.Artist}: {track.Album} - {track.Folder}");
                }
            }
        }

        private static void CreateFaultyTrackList()
        {
            //// I have a problem with Tracks. If the track number was in the format 01 A or 01. A or 1. A
            //// then it has been left in the name field.
            //// Go through tracks and try to make a list of tracks that are faulty. See all the variations
            //// and then try to remove them.
            List<Track> tracks = new List<Track>();
            tracks = _td.GetTracks();
            tracks = tracks.OrderBy(t => t.Name).ToList();

            foreach (Track track in tracks)
            {
                Console.WriteLine($"**{track.TrackId}** {track.Name}");
            }
        }

        private static Dictionary<int, string> ReadInFaultyTracks()
        {
            Dictionary<int, string> namesDictionary = new Dictionary<int, string>();

            string filePath = "names.txt";
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] keyValue = line.Split(':');
                int key = int.Parse(keyValue[0]);
                string value = keyValue[1];
                namesDictionary.Add(key, value);
            }

            return namesDictionary;
        }

        private static void FixNames(Dictionary<int, string> namesDictionary)
        {
            List<Track> tracks = TrackData.GetTracks();

            foreach (var item in namesDictionary)
            {
                var trackId = item.Key;
                var faultyTrackPart = item.Value.Replace("\"", string.Empty);

                var track = tracks.Find(t => t.TrackId == trackId);

                if (track != null && !string.IsNullOrEmpty(track.Name))
                {
                    track.Name = track.Name.Replace(faultyTrackPart, string.Empty);
                    TrackData.UpdateTrackName(track);
                    Console.WriteLine($"{track.TrackId} - {track.Name}.");
                }
            }
        }

        private static void FixTitles(Dictionary<int, string> titlesDictionary)
        {
            List<Track> tracks = TrackData.GetTracks();

            foreach (var item in titlesDictionary)
            {
                var trackId = item.Key;
                var faultyTitlePart = item.Value.Replace("\"", string.Empty);

                var track = tracks.Find(t => t.TrackId == trackId);

                if (track != null && !string.IsNullOrEmpty(track.Title))
                {
                    track.Title = track.Title.Replace(faultyTitlePart, string.Empty);
                    TrackData.UpdateTitleName(track);
                    Console.WriteLine($"{track.TrackId} - {track.Title}.");
                }
            }
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