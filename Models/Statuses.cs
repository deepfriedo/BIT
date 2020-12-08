using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class Statuses : List<Status>
    {
        public Statuses()
        {
            SQLHelper db = new SQLHelper("BIT");
            string sqlStr = "select StatusName from STATUS";
            DataTable statusNamesTable = db.ExecuteSQL(sqlStr);
            foreach (DataRow dr in statusNamesTable.Rows)
            {
                Status status = new Status(dr);
                this.Add(status);
            }
        }
    }
}
