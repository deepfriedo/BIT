using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class Skills : List<Skill>
    {
        public Skills()
        {
            SQLHelper db = new SQLHelper("BIT");

            string sqlStr = "select SkillName from Skill";

            DataTable skillNamesTable = db.ExecuteSQL(sqlStr);

            foreach (DataRow dr in skillNamesTable.Rows)
            {
                Skill skill = new Skill(dr);
                this.Add(skill);
            }
        }
    }
}
