using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class AvailableCandidate
    {
        public int RequestId { get; set; }
        public string SkillName { get; set; }
        public string Suburb { get; set; }
        public int ContractorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WeekDayName { get; set; }
        public DateTime AvailableDate { get; set; }
        private SQLHelper _db;

        public AvailableCandidate()
        {
            _db = new SQLHelper("BIT");
        }

        public AvailableCandidate(DataRow dr)
        {
            RequestId = Convert.ToInt32(dr["RequestId"]);
            SkillName = dr["SkillName"].ToString();
            Suburb = dr["Suburb"].ToString();
            ContractorId = Convert.ToInt32(dr["ContractorId"]);
            FirstName = dr["FirstName"].ToString();
            LastName = dr["LastName"].ToString();
            WeekDayName = dr["WeekDayName"].ToString();
            AvailableDate = Convert.ToDateTime(dr["Date"]);
            _db = new SQLHelper("BIT");
        }
    }
}
