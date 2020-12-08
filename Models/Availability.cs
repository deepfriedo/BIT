using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBITProject.Models
{
    public class Availability
    {
        public string WeekDayName { get; set; }

        public Availability(DataRow dr)
        {
            WeekDayName = dr["WeekDayName"].ToString();
        }
    }
}
