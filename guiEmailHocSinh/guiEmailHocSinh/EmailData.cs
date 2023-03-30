using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace guiEmailHocSinh
{
    public class EmailData
    {
        public int total { get; set; }
        public int size { get; set; }
        public List<data_> data { get; set; }

    }
    public class data_
    {

        /*public string studentName { get ;set; }
        public string studentClass { get; set; }
        public string studentEmail { get; set; }*/
        public string student_id { get; set; }
        public string tempReading { get; set; }
        public string tempTime { get; set; }
        public string status { get; set; }
        public string tempRemarks { get; set; }
        public string tempDate { get; set; }
    }
}
