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
    public class Client : ObservableObject, IDataErrorInfo
    {
        private int _clientId;
        private string _firstName;
        private string _lastName;
        private string _street;
        private string _suburb;
        private string _postcode;
        private string _state;
        private string _email;
        private string _phone;
        private DateTime _dob;
        private string _password;
        private SQLHelper _db;
        public string Error { get { return null; } }
        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();

        public string this[string name]
        {
            get
            {
                DateTime dob = Convert.ToDateTime(DOB);
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
                    case "State":
                        if (string.IsNullOrWhiteSpace(State))
                        {
                            result = "State cannot be empty!";
                        }
                        else if (State.Length < 3)
                        {
                            result = "State must be a minimum of 3 characters.";
                        }
                        break;
                    case "Email":
                        if (string.IsNullOrWhiteSpace(Email))
                        {
                            result = "Email cannot be empty!";
                        }
                        else if (Email.Length < 8)
                        {
                            result = "Email must be a minimum of 8 characters.";
                        }
                        break;
                    case "Phone":
                        if (string.IsNullOrWhiteSpace(Phone))
                        {
                            result = "Phone cannot be empty!";
                        }
                        else if (Phone.Length < 10)
                        {
                            result = "Phone must be a minimum of 10 characters.";
                        }
                        break;
                    case "DOB":
                        if (dob == DateTime.Today)
                        {
                            result = "Date of Birth cannot be Today!";

                            FileLogger fileLogger = new FileLogger();
                            fileLogger.Log(" SelectedClient.DOB Error Occured");
                        }
                        break;
                    case "Password":
                        if (string.IsNullOrWhiteSpace(Password))
                        {
                            result = "Password cannot be empty!";
                        }
                        else if (Password.Length < 6)
                        {
                            result = "Password must be a minimum of 6 characters.";
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

        public int ClientId
        {
            get { return _clientId; }
            set { _clientId = value; }
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
        public string State
        {
            get { return _state; }
            set { _state = value; OnPropertyChanged("State"); }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged("Email"); }
        }
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; OnPropertyChanged("Phone"); }
        }
        public DateTime DOB
        {
            get { return _dob; }
            set { _dob = value; OnPropertyChanged("DOB"); }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged("Password"); }
        }

        public Client()
        {
            _db = new SQLHelper("BIT");
        }

        public Client(DataRow dr)
        {
            ClientId = Convert.ToInt32(dr["clientid"]);
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
            _db = new SQLHelper("BIT");
        }
    }
}
