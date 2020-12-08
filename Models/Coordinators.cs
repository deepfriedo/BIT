using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class Coordinators : List<Coordinator>
    {
        public Coordinators()
        {
            SQLHelper db = new SQLHelper("BIT");
            string sql = "select c.CoordinatorId, firstname, lastname, street, suburb, postcode, state, email, phone, dateofbirth, password from BIT_USER b, COORDINATOR c where c.CoordinatorId = b.UserId";
            DataTable coordinatorsTable = db.ExecuteSQL(sql);

            foreach (DataRow dr in coordinatorsTable.Rows)
            {
                Coordinator coordinator = new Coordinator(dr);
                this.Add(coordinator);
            }
        }
    }
}
