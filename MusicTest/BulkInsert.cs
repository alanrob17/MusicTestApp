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
            //// Use this to save the data to disc in JSON format.
            // SerialiseData();

            //// Second section
            // Read the data from disc.
            List<Artist> artists = new();
            artists = _ad.DeserialiseArtists(artists);

            List<Record> records = new();
            records = _rd.DeserialiseRecords(records);


            List<Disk> discs = new();
            discs = _dd.DeserialiseDiscs(discs);

            discs = discs.OrderByDescending(d => d.Duration).ToList();

            // I found TimeSpans larger than 1 day. They won't go back in the db.
            foreach (Disk disc in discs)
            {
                if (disc.Duration > TimeSpan.FromDays(1)) // compare with TimeSpan representing 1 day
                {
                    disc.Duration -= TimeSpan.FromDays(1);
                    Console.WriteLine($"{disc.DiscId} - {disc.Duration} - {disc.Name}");
                }
            }

            List<Track> tracks = new();
            tracks = _td.DeserialiseTracks(tracks);

            //// Third section
            // _ad.BulkInsert(artists);
            // _rd.BulkInsert(records);
            // _dd.BulkInsert(discs);
            // _td.BulkInsert(tracks);
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