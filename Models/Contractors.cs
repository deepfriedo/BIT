using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class Contractors : List<Contractor>
    {
        public Contractors()
        {
            SQLHelper db = new SQLHelper("BIT");
            string sql = "select c.ContractorId, firstname, lastname, street, suburb, postcode, state, email, phone, dateofbirth, password, active from BIT_USER b, CONTRACTOR c where c.ContractorId = b.UserId";
            DataTable contractorsTable = db.ExecuteSQL(sql);

            foreach (DataRow dr in contractorsTable.Rows)
            {
                Contractor contractor = new Contractor(dr);
                this.Add(contractor);
            }
        }
    }
}
