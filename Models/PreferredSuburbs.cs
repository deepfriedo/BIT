using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class PreferredSuburbs : List<PreferredSuburb>
    {
        public PreferredSuburbs(int contractorId)
        {
            SQLHelper db = new SQLHelper("BIT");
            string sqlStr = "select Suburb from PREFERRED_SUBURB ps where ps.ContractorId = " + contractorId;
            DataTable suburbsTable = db.ExecuteSQL(sqlStr);
            foreach (DataRow dr in suburbsTable.Rows)
            {
                PreferredSuburb preferredSuburb = new PreferredSuburb(dr);
                this.Add(preferredSuburb);
            }
        }
    }
}
