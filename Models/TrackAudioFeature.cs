namespace SpotifyAnalyzer.Models
{
    public class TrackAudioFeature
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public float Acousticness { get; set; }

        public float Danceability { get; set; }

        public int DurationMs { get; set; }

        public float Instrumentalness { get; set; }

        public float Liveness { get; set; }

        public float Loudness { get; set; }

        public int Mode { get; set; }

        public float Speechiness { get; set; }

        public float Tempo { get; set; }

        public float Valence { get; set; }
    }
}
