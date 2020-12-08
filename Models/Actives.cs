using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class Actives : List<Active>
    {
        public Actives()
        {
            SQLHelper db = new SQLHelper("BIT");
            string sqlStr = "select ActiveName from ACTIVE";
            DataTable activeNamesTable = db.ExecuteSQL(sqlStr);
            foreach (DataRow dr in activeNamesTable.Rows)
            {
                Active active = new Active(dr);
                this.Add(active);
            }
        }
    }
}
