using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LifeStyle.DataBase
{
    public struct DBParameters
    {
        [JsonPropertyName("Host")]
        public string Host { get; set; }
        [JsonPropertyName("Username")]
        public string Username { get; set; }
        [JsonPropertyName("Password")]
        public string Password { get; set; }
        [JsonPropertyName("Database")]
        public string Database { get; set; }
    }
}
