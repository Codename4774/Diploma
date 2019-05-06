using System;
using System.Collections.Generic;
using System.Text;

namespace PublicTransport.Backend.Models
{
    public class FavoriteStop
    {
        public string direction { get; set; }
        public string[][] times { get; set; }
        public string route_id { get; set; }
        public string stop_name { get; set; }
        public string route_short_name { get; set; }
        public string route_long_name { get; set; }
        public int route_type { get; set; }
        public string stop_id { get; set; }
    }
}
