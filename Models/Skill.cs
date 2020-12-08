using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class Skill : ObservableObject
    {
        private string _skillName;
        private SQLHelper _db;

        public string SkillName
        {
            get { return _skillName; }
            set { _skillName = value; OnPropertyChanged("SkillName"); }
        }
        public Skill()
        {
            _db = new SQLHelper("BIT");
        }
        public Skill(DataRow dr)
        {
            SkillName = dr["skillname"].ToString();
        }
    }
}
