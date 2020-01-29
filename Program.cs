using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using SpotifyAnalyzer.Models;
using SpotifyAPI.Web;

namespace SpotifyAnalyzer
{
    public static class Program
    {
        private static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        private static string checkDirectoryPath(string directoryPath)
        {
            if (!directoryPath.EndsWith("/", StringComparison.Ordinal))
            {
                directoryPath += "/";
            }

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Please enter a valid directory path:");
                directoryPath = Console.ReadLine();

                checkDirectoryPath(directoryPath);
            }

            return directoryPath;
        }


        public static async System.Threading.Tasks.Task Main()
        {
            Console.WriteLine("Save Excel Report to:");
            string directoryPath = Console.ReadLine();

            directoryPath = checkDirectoryPath(directoryPath);

            Services.SpotifyService spotifyService = new Services.SpotifyService();

            Service.VisualizerService visualizerService = new Service.VisualizerService();

            Dictionary<string,string> top50UsaPlaylistDict;

            using (SpotifyWebAPI spotify = await spotifyService.GetWebAPIAsync())
            {
                top50UsaPlaylistDict = await spotifyService.GetTop50UsaPlaylistDictAsync(spotify);

                List<TrackAudioFeature> trackAudioFeatureList = await spotifyService.GetTrackAudioFeaturesAsync(spotify, top50UsaPlaylistDict);

                DataTable dt = CreateDataTable(trackAudioFeatureList);

                visualizerService.Get(directoryPath, dt);
            }
        }
    }
}




