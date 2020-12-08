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
    public class ScheduleViewModel : ObservableObject
    {
        private Schedule _selectedSchedule;
        public Schedule SelectedSchedule
        {
            get { return _selectedSchedule; }
            set { _selectedSchedule = value; OnPropertyChanged("SelectedSchedule"); }
        }

        private ObservableCollection<Schedule> _schedules;
        public ObservableCollection<Schedule> Schedules
        {
            get { return _schedules; }
            set { _schedules = value; OnPropertyChanged("Schedules"); }
        }

        private Status _selectedStatus;
        public Status SelectedStatus
        {
            get { return _selectedStatus; }
            set { _selectedStatus = value; OnPropertyChanged("SelectedStatus"); }
        }

        private ObservableCollection<Status> _statues;
        public ObservableCollection<Status> Statuses
        {
            get { return _statues; }
            set { _statues = value; OnPropertyChanged("Statuses"); }
        }

        public ScheduleViewModel()
        {
            LoadSchedules();
            LoadStatuses();
        }

        public void LoadSchedules()
        {
            Schedules allSchedules = new Schedules();
            Schedules = new ObservableCollection<Schedule>(allSchedules);
        }

        public void LoadStatuses()
        {
            Statuses allStatuses = new Statuses();
            Statuses = new ObservableCollection<Status>(allStatuses);
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

        private MyCommand _findAcceptedCommand;
        public MyCommand FindAcceptedCommand
        {
            get
            {
                if (_findAcceptedCommand == null)
                {
                    _findAcceptedCommand = new MyCommand(this.FindAcceptedMethod, true);
                }
                return _findAcceptedCommand;
            }
            set
            {
                _findAcceptedCommand = value;
            }
        }

        private MyCommand _findRejectedCommand;
        public MyCommand FindRejectedCommand
        {
            get
            {
                if (_findRejectedCommand == null)
                {
                    _findRejectedCommand = new MyCommand(this.FindRejectedMethod, true);
                }
                return _findRejectedCommand;
            }
            set
            {
                _findRejectedCommand = value;
            }
        }

        private MyCommand _findCompletedCommand;
        public MyCommand FindCompletedCommand
        {
            get
            {
                if (_findCompletedCommand == null)
                {
                    _findCompletedCommand = new MyCommand(this.FindCompletedMethod, true);
                }
                return _findCompletedCommand;
            }
            set
            {
                _findCompletedCommand = value;
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

        private MyCommand _findVerifiedCommand;
        public MyCommand FindVerifiedCommand
        {
            get
            {
                if (_findVerifiedCommand == null)
                {
                    _findVerifiedCommand = new MyCommand(this.FindVerifiedMethod, true);
                }
                return _findVerifiedCommand;
            }
            set
            {
                _findVerifiedCommand = value;
            }
        }

        public void FindScheduledMethod()
        {
            ScheduledSchedules allScheduledSchedules = new ScheduledSchedules();
            Schedules = new ObservableCollection<Schedule>(allScheduledSchedules);
        }

        public void FindAcceptedMethod()
        {
            AcceptedSchedules allAcceptedSchedules = new AcceptedSchedules();
            Schedules = new ObservableCollection<Schedule>(allAcceptedSchedules);
        }

        public void FindRejectedMethod()
        {
            RejectedSchedules allRejectedSchedules = new RejectedSchedules();
            Schedules = new ObservableCollection<Schedule>(allRejectedSchedules);
        }

        public void FindCompletedMethod()
        {
            CompletedSchedules allCompletedSchedules = new CompletedSchedules();
            Schedules = new ObservableCollection<Schedule>(allCompletedSchedules);
        }

        public void FindAllMethod()
        {
            Schedules allSchedules = new Schedules();
            Schedules = new ObservableCollection<Schedule>(allSchedules);
        }

        public void FindVerifiedMethod()
        {
            VerifiedSchedules allVerifiedSchedules = new VerifiedSchedules();
            Schedules = new ObservableCollection<Schedule>(allVerifiedSchedules);
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
            Schedules allSchedules = new Schedules();
            Schedules = new ObservableCollection<Schedule>(allSchedules);
        }

        public void UpdateMethod()
        {
            string sqlStr = "update SCHEDULED set StartDate = '" + Convert.ToDateTime(SelectedSchedule.StartDate).ToString("yyyy-MM-dd") +
                "', KilometersTravelled = '" + SelectedSchedule.KilometersTravelled +
                "', HoursWorked = '" + SelectedSchedule.HoursWorked +
                "', Status = '" + SelectedStatus.StatusName +
                "', EndDate = '" + Convert.ToDateTime(SelectedSchedule.EndDate).ToString("yyyy-MM-dd") +
                "' where RequestId = " + SelectedSchedule.RequestId +
                " AND ContractorId = " + SelectedSchedule.ContractorId;
                
            SQLHelper objHelper = new SQLHelper("BIT");
            objHelper.ExecuteNonQuery(sqlStr);
        }
    }
}
