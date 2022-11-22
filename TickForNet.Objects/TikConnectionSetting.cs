using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TickForNet.Objects
{
    public class TikConnectionSetting
    {
        public static TikConnectionSetting ReadFromConnectionString(string connStr)
        {
            var conn = new TikConnectionSetting();
            var connParts = connStr.Split(';').ToList();

            conn.TikHost = connParts.FirstOrDefault(c => c.StartsWith("host="))?.Substring(5);
            conn.TikUser = connParts.FirstOrDefault(c => c.StartsWith("username="))?.Substring(9);
            conn.TikPassword = connParts.FirstOrDefault(c => c.StartsWith("password="))?.Substring(9);

            return conn;
        }
        public string TikHost { get; set; }
        public string TikUser { get; set; }
        public string TikPassword { get; set; }
    }
}
