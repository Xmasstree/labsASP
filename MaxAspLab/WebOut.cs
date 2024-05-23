using System.Text.Json.Serialization;

namespace MaxAspLab
{
    public class WebOut
    {
        public string st1 {  get; set; }
        public Dictionary<char, int> letters { get; set; }
        public string st2 {  get; set; }
        public string sortST { get; set; }
        public string rand { get; set;}
        public string shortst { get; set; }
    }

    public class FileJsonChar
    {
        [JsonPropertyName("Blacklist")]
        public string[] Blacklist { get; set; }
    }

    public class FileJson
    {
        [JsonPropertyName("RandomApi")]
        public string RandomApi { get; set; }
        public FileJsonChar Settings { get; set; }
    }
}
