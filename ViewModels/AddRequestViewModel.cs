using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Models;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.ViewModels
{
    public class AddRequestViewModel : ObservableObject
    {
        private Request _request;

        public Request Request
        {
            get { return _request; }
            set { _request = value; OnPropertyChanged("Request"); }
        }

        public AddRequestViewModel()
        {
            Request = new Request();
        }

        private MyCommand _addCommand;
        public MyCommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new MyCommand(this.AddMethod, true);
                }
                return _addCommand;
            }
            set
            {
                _addCommand = value;
            }
        }

        private MyCommand _cancelCommand;
        public MyCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new MyCommand(this.CancelMethod, true);
                }
                return _cancelCommand;
            }
            set
            {
                _cancelCommand = value;
            }
        }

        public void CancelMethod()
        {
            Request = new Request();
        }

        public void AddMethod()
        {
            string sqlStr = "DECLARE @NewPrimaryKeyLocation INT INSERT INTO LOCATION(Street, Suburb, Postcode, ClientId) values('" + Request.Street + "','" + Request.Suburb + "','" + Request.Postcode + "', " + Request.ClientId + ") SET @NewPrimaryKeyLocation = @@IDENTITY INSERT INTO REQUEST(LocationId, SkillName, EarliestStartDate, DueDate, Comment, Status) VALUES(@NewPrimaryKeyLocation, '" + Request.SkillName + "', '" + Convert.ToDateTime(Request.StartDate).ToString("yyyy-MM-dd") + "', '" + Convert.ToDateTime(Request.DueDate).ToString("yyyy-MM-dd") + "', '" + Request.Comment + "', 'Pending')";

            SQLHelper objHelper = new SQLHelper("BIT");
            objHelper.ExecuteNonQuery(sqlStr);
        }
    }
}
