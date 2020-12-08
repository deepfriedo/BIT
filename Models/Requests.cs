using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class Requests : List<Request>
    {
        public Requests()
        {
            SQLHelper db = new SQLHelper("BIT");
            string sql = "select r.RequestId, l.ClientId, b.FirstName, b.LastName, r.SkillName, l.Street, l.Suburb, l.Postcode, r.EarliestStartDate, r.DueDate, r.Comment, r.Status  from BIT_USER b, REQUEST r, CLIENT c, LOCATION l where b.UserId = c.ClientId AND c.ClientId = l.ClientId AND l.LocationId = r.LocationId";
            DataTable requestsTable = db.ExecuteSQL(sql);

            foreach (DataRow dr in requestsTable.Rows)
            {
                Request request = new Request(dr);
                this.Add(request);
            }
        }
    }
}
