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
    public class Request : ObservableObject, IDataErrorInfo
    {
        private string _firstName;
        private string _lastName;
        private string _skillName;
        private string _street;
        private string _suburb;
        private string _postcode;
        private DateTime _startDate;
        private DateTime _dueDate;
        private string _comment;
        private string _status;
        private SQLHelper _db;
        public int RequestId { get; set; }
        public int ClientId { get; set; }
        public int LocationId { get; set; }
        public string Error { get { return null; } }
        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();

        public string this[string name]
        {
            get
            {
                DateTime start = Convert.ToDateTime(StartDate);
                DateTime end = Convert.ToDateTime(DueDate);

                string result = null;
                switch (name)
                {
                    case "FirstName":
                        if (string.IsNullOrWhiteSpace(FirstName))
                        {
                            result = "First name cannot be empty!";
                        }
                        else if (FirstName.Length < 3)
                        {
                            result = "First name must be a minimum of 3 characters.";
                        }
                        break;
                    case "LastName":
                        if (string.IsNullOrWhiteSpace(LastName))
                        {
                            result = "Last name cannot be empty!";
                        }
                        else if (LastName.Length < 3)
                        {
                            result = "Last name must be a minimum of 3 characters.";
                        }
                        break;
                    case "SkillName":
                        if (string.IsNullOrWhiteSpace(SkillName))
                        {
                            result = "SkillName cannot be empty!";
                        }
                        else if (SkillName.Length < 2)
                        {
                            result = "SkillName must be a minimum of 2 characters.";
                        }
                        break;
                    case "Street":
                        if (string.IsNullOrWhiteSpace(Street))
                        {
                            result = "Street cannot be empty!";
                        }
                        else if (Street.Length < 5)
                        {
                            result = "Street must be a minimum of 5 characters.";
                        }
                        break;
                    case "Suburb":
                        if (string.IsNullOrWhiteSpace(Suburb))
                        {
                            result = "Suburb cannot be empty!";
                        }
                        else if (Suburb.Length < 4)
                        {
                            result = "Suburb must be a minimum of 4 characters.";
                        }
                        break;
                    case "Postcode":
                        if (string.IsNullOrWhiteSpace(Postcode))
                        {
                            result = "Postcode cannot be empty!";
                        }
                        else if (Postcode.Length < 4)
                        {
                            result = "Postcode must be a minimum of 4 characters.";
                        }
                        break;
                    case "StartDate":
                        if (start < DateTime.Today)
                        {
                            result = "Earliest Start Date cannot be Yesterday!";
                        }
                        break;
                    case "DueDate":
                        if (end == DateTime.Today || end < DateTime.Today)
                        {
                            result = "Due Date cannot be Today or Yesterday!";
                        }
                        break;
                    case "Comment":
                        if (string.IsNullOrWhiteSpace(Comment))
                        {
                            result = "Comment cannot be empty!";
                        }
                        else if (Comment.Length < 6)
                        {
                            result = "Comment must be a minimum of 6 characters.";
                        }
                        break;
                }
                if (ErrorCollection.ContainsKey(name))
                {
                    ErrorCollection[name] = result;
                }
                else if (result != null)
                {
                    ErrorCollection.Add(name, result);
                }
                OnPropertyChanged("ErrorCollection");
                return result;
            }
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
        public string SkillName
        {
            get { return _skillName; }
            set { _skillName = value; OnPropertyChanged("SkillName"); }
        }
        public string Street
        {
            get { return _street; }
            set { _street = value; OnPropertyChanged("Street"); }
        }
        public string Suburb
        {
            get { return _suburb; }
            set { _suburb = value; OnPropertyChanged("Suburb"); }
        }
        public string Postcode
        {
            get { return _postcode; }
            set { _postcode = value; OnPropertyChanged("Postcode"); }
        }
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; OnPropertyChanged("StartDate"); }
        }
        public DateTime DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; OnPropertyChanged("DueDate"); }
        }
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; OnPropertyChanged("Comment"); }
        }
        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Status"); }
        }

        public Request()
        {
            _db = new SQLHelper("BIT");
        }

        public Request(DataRow dr)
        {
            RequestId = Convert.ToInt32(dr["requestid"]);
            ClientId = Convert.ToInt32(dr["clientid"]);
            FirstName = dr["FirstName"].ToString();
            LastName = dr["LastName"].ToString();
            SkillName = dr["skillname"].ToString();
            Street = dr["street"].ToString();
            Suburb = dr["suburb"].ToString();
            Postcode = dr["postcode"].ToString();
            StartDate = Convert.ToDateTime(dr["earlieststartdate"]);
            DueDate = Convert.ToDateTime(dr["duedate"]);
            Comment = dr["comment"].ToString();
            Status = dr["status"].ToString();
            _db = new SQLHelper("BIT");
        }
    }
}
