using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class PreferredSkills : List<PreferredSkill>
    {
        public PreferredSkills(int contractorId)
        {
            SQLHelper db = new SQLHelper("BIT");
            string sqlStr = "select SkillName from PREFERRED_SKILL ps where ps.ContractorId = " + contractorId;
            DataTable skillNamesTable = db.ExecuteSQL(sqlStr);
            foreach (DataRow dr in skillNamesTable.Rows)
            {
                PreferredSkill preferredSkill = new PreferredSkill(dr);
                this.Add(preferredSkill);
            }
        }
    }
}
