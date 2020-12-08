using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class ActiveContractors : List<Contractor>
    {
        public ActiveContractors()
        {
            SQLHelper db = new SQLHelper("BIT");
            string sql = "select c.ContractorId, firstname, lastname, street, suburb, postcode, state, email, phone, dateofbirth, password, active from BIT_USER b, CONTRACTOR c where c.ContractorId = b.UserId AND (c.Active = 'Available' OR c.Active = 'In Progress')";
            DataTable contractorsTable = db.ExecuteSQL(sql);

            foreach (DataRow dr in contractorsTable.Rows)
            {
                Contractor contractor = new Contractor(dr);
                this.Add(contractor);
            }
        }
    }
}
