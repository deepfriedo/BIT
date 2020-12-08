using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class Schedule : ObservableObject
    {
        private string _status;
        private int _kilometersTravelled;
        private int _hoursWorked;
        private DateTime _startDate;
        private DateTime _endDate;
        private SQLHelper _db;

        public int RequestId { get; private set; }
        public int ContractorId { get; private set; }
        public int CoordinatorId { get; private set; }

        private string _skillName;
        private string _suburb;
        private string _firstName;
        private string _lastName;
        public string WeekDayName { get; private set; }

        public string SkillName
        {
            get { return _skillName; }
            set { _skillName = value; OnPropertyChanged("SkillName"); }
        }

        public string Suburb
        {
            get { return _suburb; }
            set { _suburb = value; OnPropertyChanged("Suburb"); }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged("FirstName"); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; OnPropertyChanged("LastName"); }
        }



        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Status"); }
        }
        public int KilometersTravelled
        {
            get { return _kilometersTravelled; }
            set { _kilometersTravelled = value; OnPropertyChanged("KilometersTravelled"); }
        }
        public int HoursWorked
        {
            get { return _hoursWorked; }
            set { _hoursWorked = value; OnPropertyChanged("HoursWorked"); }
        }
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; OnPropertyChanged("StartDate"); }
        }
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; OnPropertyChanged("EndDate"); }
        }

        public Schedule()
        {
            _db = new SQLHelper("BIT");
        }

        public Schedule(DataRow dr)
        {
            RequestId = Convert.ToInt32(dr["requestid"]);
            SkillName = dr["SkillName"].ToString();
            Suburb = dr["Suburb"].ToString();
            ContractorId = Convert.ToInt32(dr["contractorid"]);
            FirstName = dr["FirstName"].ToString();
            LastName = dr["LastName"].ToString();
            WeekDayName = dr["WeekDayName"].ToString();
            StartDate = Convert.ToDateTime(dr["startdate"]);
            KilometersTravelled = Convert.ToInt32(dr["KilometersTravelled"]);
            HoursWorked = Convert.ToInt32(dr["HoursWorked"]);
            CoordinatorId = Convert.ToInt32(dr["coordinatorid"]);
            Status = dr["status"].ToString();
            EndDate = Convert.ToDateTime(dr["enddate"]);
            _db = new SQLHelper("BIT");
        }
    }
}
