using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Models;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.ViewModels
{
    public class RequestViewModel : ObservableObject
    {
        private Request _selectedRequest;
        public Request SelectedRequest
        {
            get { return _selectedRequest; }
            set { _selectedRequest = value; OnPropertyChanged("SelectedRequest"); }
        }

        private ObservableCollection<Request> _requests;
        public ObservableCollection<Request> Requests
        {
            get { return _requests; }
            set { _requests = value; OnPropertyChanged("Requests"); }
        }

        public RequestViewModel()
        {
            PendingRequests allPendingRequests = new PendingRequests();
            Requests = new ObservableCollection<Request>(allPendingRequests);
        }

        private MyCommand _findScheduledCommand;
        public MyCommand FindScheduledCommand
        {
            get
            {
                if (_findScheduledCommand == null)
                {
                    _findScheduledCommand = new MyCommand(this.FindScheduledMethod, true);
                }
                return _findScheduledCommand;
            }
            set
            {
                _findScheduledCommand = value;
            }
        }

        private MyCommand _findAllCommand;
        public MyCommand FindAllCommand
        {
            get
            {
                if (_findAllCommand == null)
                {
                    _findAllCommand = new MyCommand(this.FindAllMethod, true);
                }
                return _findAllCommand;
            }
            set
            {
                _findAllCommand = value;
            }
        }

        private MyCommand _findPendingCommand;
        public MyCommand FindPendingCommand
        {
            get
            {
                if (_findPendingCommand == null)
                {
                    _findPendingCommand = new MyCommand(this.FindPendingMethod, true);
                }
                return _findPendingCommand;
            }
            set
            {
                _findPendingCommand = value;
            }
        }

        public void FindScheduledMethod()
        {
            ScheduledRequests allScheduledRequests = new ScheduledRequests();
            Requests = new ObservableCollection<Request>(allScheduledRequests);
        }

        public void FindAllMethod()
        {
            Requests allRequests = new Requests();
            Requests = new ObservableCollection<Request>(allRequests);
        }

        public void FindPendingMethod()
        {
            PendingRequests allPendingRequests = new PendingRequests();
            Requests = new ObservableCollection<Request>(allPendingRequests);
        }

        private MyCommand _updateCommand;
        public MyCommand UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                {
                    _updateCommand = new MyCommand(this.UpdateMethod, true);
                }
                return _updateCommand;
            }
            set
            {
                _updateCommand = value;
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
            Requests allRequests = new Requests();
            Requests = new ObservableCollection<Request>(allRequests);
        }

        public void UpdateMethod()
        {
            string sqlStr = "update REQUEST set SkillName = '" + SelectedRequest.SkillName +
                "', EarliestStartDate = '" + Convert.ToDateTime(SelectedRequest.StartDate).ToString("yyyy-MM-dd") +
                "', DueDate = '" + Convert.ToDateTime(SelectedRequest.DueDate).ToString("yyyy-MM-dd") +
                "', Comment = '" + SelectedRequest.Comment +
                "', Status = '" + SelectedRequest.Status +
                "' where RequestId = " + SelectedRequest.RequestId +
                " update LOCATION set Street = '" + SelectedRequest.Street +
                "', Suburb = '" + SelectedRequest.Suburb +
                "', Postcode = '" + SelectedRequest.Postcode +
                "' where LocationId = " + SelectedRequest.LocationId;

            SQLHelper objHelper = new SQLHelper("BIT");
            objHelper.ExecuteNonQuery(sqlStr);
        }
    }
}
