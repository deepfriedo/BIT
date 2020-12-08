using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class RejectedSchedules : List<Schedule>
    {
        public RejectedSchedules()
        {
            SQLHelper db = new SQLHelper("BIT");
            string sql = "select s.RequestId, r.SkillName, l.Suburb, s.ContractorId, b.FirstName, b.LastName, s.StartDate, DATENAME(dw, s.StartDate) as WeekDayName, s.KilometersTravelled, s.HoursWorked, s.CoordinatorId, s.Status, s.EndDate from BIT_USER b, REQUEST r, CONTRACTOR c, SCHEDULED s, COORDINATOR cr, LOCATION l where b.UserId = c.ContractorId AND c.ContractorId = s.ContractorId AND s.RequestId = r.RequestId AND r.LocationId = l.LocationId AND cr.CoordinatorId = s.CoordinatorId AND s.Status = 'Rejected'";
            DataTable schedulesTable = db.ExecuteSQL(sql);

            foreach (DataRow dr in schedulesTable.Rows)
            {
                Schedule schedule = new Schedule(dr);
                this.Add(schedule);
            }
        }
    }
}
