using System;
using System.Collections.Generic;
using System.Text;

namespace WebCommon.Model
{
    public class DatabaseTable
    {
        public string name { get; set; }
        public long object_id { get; set; }
        public DateTime create_date { get; set; }
    }
}
