using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace guiEmailHocSinh
{
    public class EmailData2
    {
        public int total { get; set; }
        public int size { get; set; }
        public List<data2_> data { get; set; }
    }
    public class data2_
    {
        public string id { get; set; }
        public string class_name { get; set; }
   
    }
}
