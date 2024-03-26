using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data
{
    #region /api/movies
    public partial class ApiMovies
    {
        public string Keyword { get; set; }
        public Movie[] Movies { get; set; }
        public Pagination Pagination { get; set; }
    }

    public partial class Movie
    {
        public string Date { get; set; }
        public string Id { get; set; }
        public string Img { get; set; }
        public string[] Tags { get; set; }
        public string Title { get; set; }
    }

    public partial class Pagination
    {
        public long? CurrentPage { get; set; }
        public bool? HasNextPage { get; set; }
        public long? NextPage { get; set; }
        public long[] Pages { get; set; }
    }
    #endregion


    #region Magnet And Serialize
    partial class Magnet
    {
        public static ApiMovies FromJson(string json) => JsonConvert.DeserializeObject<ApiMovies>(json, Data.Converter.Settings);
    }

    public static class Serialize
    {
        static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
        public static string ToJson(this ApiMovies self) => JsonConvert.SerializeObject(self, Settings);
    }
    #endregion
}
