using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Models;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.ViewModels
{
    public class AddScheduleViewModel : RequestViewModel
    {
        private ObservableCollection<AvailableCandidate> _availableCandidates;
        public ObservableCollection<AvailableCandidate> AvailableCandidates
        {
            get { return _availableCandidates; }
            set { _availableCandidates = value; OnPropertyChanged("AvailableCandidates"); }
        }

        public AvailableCandidate SelectedCandidate { get; set; }

        private MyCommand _findCommand;
        public MyCommand FindCommand
        {
            get
            {
                if (_findCommand == null)
                {
                    _findCommand = new MyCommand(this.FindMethod, true);
                }
                return _findCommand;
            }
            set
            {
                _findCommand = value;
            }
        }

        private MyCommand _confirmCommand;
        public MyCommand ConfirmCommand
        {
            get
            {
                if (_confirmCommand == null)
                {
                    _confirmCommand = new MyCommand(this.AddMethod, true);
                }
                return _confirmCommand;
            }
            set
            {
                _confirmCommand = value;
            }
        }

        public void AddMethod()
        {
            string sqlStr = "Insert Into SCHEDULED(requestid, contractorid, startdate, status, kilometersTravelled, hoursworked, coordinatorid, enddate) values(" + SelectedCandidate.RequestId + ", " + SelectedCandidate.ContractorId + ", '" + SelectedCandidate.AvailableDate.ToString("yyyy-MM-dd") + "', 'Scheduled', 0, 0, '106', '0001-01-01') ";

            SQLHelper objHelper = new SQLHelper("BIT");
            objHelper.ExecuteNonQuery(sqlStr);

            string updateStr = "update REQUEST set Status = 'Scheduled' where RequestId = " + SelectedCandidate.RequestId;
            objHelper.ExecuteNonQuery(updateStr);
        }

        public void FindMethod()
        {
            AvailableCandidates allCandidates = new AvailableCandidates(SelectedRequest.RequestId);
            AvailableCandidates = new ObservableCollection<AvailableCandidate>(allCandidates);
        }
    }
}
