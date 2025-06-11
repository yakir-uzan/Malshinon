using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon
{
    internal class IntelReports
    {
        public int id { get; set; }
        public int reporter_id { get; set; }
        public int target_id { get; set; }
        public string text { get; set; }
        public DateTime timestamp { get; set; }
    

    public IntelReports(int id, int reporter_id, int target_id, string text, DateTime timestamp)
        {
            this.id = id;
            this.reporter_id = reporter_id;
            this.target_id = target_id;
            this.text = text;
            this.timestamp = timestamp;
        }
    }
}
