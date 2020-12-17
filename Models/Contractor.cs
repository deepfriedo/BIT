using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using WpfBITProject.Data_Access_Layer;
using NLog;

namespace WpfBITProject.Models
{
    public class Contractor : BaseUser
    {
        private int _contractorId;
        private SQLHelper _db;
        private PreferredSkills _preferredSkills;
        private string _active;

        public PreferredSuburbs PreferredSuburbs { get; set; }
        public Availabilities Availabilities { get; set; }
        public Skills Skills { get; set; }

        public int ContractorId
        {
            get { return _contractorId; }
            set { _contractorId = value; }
        }
        public PreferredSkills PreferredSkills
        {
            get { return _preferredSkills; }
            set { _preferredSkills = value; OnPropertyChanged("PreferredSkills"); }
        }
        public string Active
        {
            get { return _active; }
            set { _active = value; OnPropertyChanged("Active"); }
        }
        
        public Contractor()
        {
            _db = new SQLHelper("BIT");
        }

        public Contractor(DataRow dr)
        {
            ContractorId = Convert.ToInt32(dr["ContractorId"]);
            PreferredSkills = new PreferredSkills(ContractorId);
            PreferredSuburbs = new PreferredSuburbs(ContractorId);
            Availabilities = new Availabilities(ContractorId);
            Skills = new Skills();
            FirstName = dr["firstname"].ToString();
            LastName = dr["lastname"].ToString();
            Street = dr["street"].ToString();
            Suburb = dr["suburb"].ToString();
            Postcode = dr["postcode"].ToString();
            State = dr["state"].ToString();
            Email = dr["email"].ToString();
            Phone = dr["phone"].ToString();
            DOB = Convert.ToDateTime(dr["dateofbirth"]);
            Password = dr["password"].ToString();
            Active = dr["Active"].ToString();
            _db = new SQLHelper("BIT");
        }
    }
}
