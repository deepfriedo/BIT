using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class Availabilities : List<Availability>
    {
        public Availabilities(int contractorId)
        {
            SQLHelper db = new SQLHelper("BIT");
            string sqlStr = "select WeekDayName from AVAILABILITY a where a.ContractorId = " + contractorId;

            DataTable availabilitiesTable = db.ExecuteSQL(sqlStr);
            foreach (DataRow dr in availabilitiesTable.Rows)
            {
                Availability availability = new Availability(dr);
                this.Add(availability);
            }
        }
    }
}
