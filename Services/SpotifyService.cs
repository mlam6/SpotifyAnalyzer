using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyAnalyzer.Models;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;

namespace SpotifyAnalyzer.Services
{
    public class SpotifyService
    {
        public async Task<SpotifyWebAPI> GetWebAPIAsync()
        {
            Client client = new Client();

            CredentialsAuth auth = new CredentialsAuth(client.Id, client.Secret);
            Token token = await auth.GetToken();

            return new SpotifyWebAPI()
            {
                AccessToken = token.AccessToken,
                TokenType = token.TokenType
            };
        }

        public async Task<Dictionary<string, string>> GetTop50UsaPlaylistDictAsync(SpotifyWebAPI spotify)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            FullPlaylist top50UsaPlaylist = await spotify.GetPlaylistAsync("37i9dQZEVXbLRQDuF5jeBp");

            if (top50UsaPlaylist.HasError())
            {
                Console.WriteLine("Error Status: " + top50UsaPlaylist.Error.Status);
                Console.WriteLine("Error Msg: " + top50UsaPlaylist.Error.Message);

                return null;
            }

            foreach (PlaylistTrack track in top50UsaPlaylist.Tracks.Items)
            {
                if(!dict.ContainsKey(track.Track.Id))
                {
                    dict.Add(track.Track.Id, track.Track.Name);
                }
            }

            return dict;
        }

        public async Task<List<TrackAudioFeature>> GetTrackAudioFeaturesAsync(SpotifyWebAPI spotify, Dictionary<string, string> top50UsaPlaylistDict)
        {
            List<string> trackIdList = new List<string>(top50UsaPlaylistDict.Keys);

            List<TrackAudioFeature> trackAudioFeatureList = new List<TrackAudioFeature>();

            SeveralAudioFeatures audioFeatureList = await spotify.GetSeveralAudioFeaturesAsync(trackIdList);

            if (audioFeatureList.HasError())
            {
                Console.WriteLine("Error Status: " + audioFeatureList.Error.Status);
                Console.WriteLine("Error Msg: " + audioFeatureList.Error.Message);

                return null;
            }

            foreach(AudioFeatures audioFeatures in audioFeatureList.AudioFeatures)
            {
                trackAudioFeatureList.Add(new TrackAudioFeature
                {
                    Id = audioFeatures.Id,
                    Name = top50UsaPlaylistDict[audioFeatures.Id],
                    Danceability = audioFeatures.Danceability,
                    DurationMs = audioFeatures.DurationMs,
                    Instrumentalness = audioFeatures.Instrumentalness,
                    Liveness = audioFeatures.Liveness,
                    Loudness = audioFeatures.Loudness,
                    Mode = audioFeatures.Mode,
                    Speechiness = audioFeatures.Speechiness,
                    Tempo = audioFeatures.Tempo,
                    Valence = audioFeatures.Valence
                });
            }

            return trackAudioFeatureList;
        }
    }
}
