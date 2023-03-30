using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace guiEmailHocSinh
{
    public class EmailData1
    {
        public int total { get; set; }
        public int size { get; set; }
        public List<data1_> data { get; set; }
    }
    public class data1_
    {
        public string id { get; set; }
        public string studentName { get; set; }
        public string studentClass { get; set; }
        public string studentEmail { get; set; }

        /*public string tempReading { get; set; }
        public string tempTime { get; set; }
        public string status { get; set; }
        public string tempRemarks { get; set; }*/
    }
}
