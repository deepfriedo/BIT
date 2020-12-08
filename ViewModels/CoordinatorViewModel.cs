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
    public class CoordinatorViewModel : ObservableObject
    {
        private ObservableCollection<Coordinator> _coordinators;
        private Coordinator _selectedCoordinator;

        public ObservableCollection<Coordinator> Coordinators
        {
            get { return _coordinators; }
            set { _coordinators = value; OnPropertyChanged("Coordinators"); }
        }

        public Coordinator SelectedCoordinator
        {
            get { return _selectedCoordinator; }
            set { _selectedCoordinator = value; OnPropertyChanged("SelectedCoordinator"); }
        }

        public CoordinatorViewModel()
        {
            GetAllCoordinators();
        }

        public virtual ObservableCollection<Coordinator> GetAllCoordinators()
        {
            Coordinators allCoordinators = new Coordinators();
            Coordinators = new ObservableCollection<Coordinator>(allCoordinators);
            return Coordinators;
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
            Coordinators allCoordinators = new Coordinators();
            Coordinators = new ObservableCollection<Coordinator>(allCoordinators);
        }

        public void UpdateMethod()
        {
            string sqlStr = "update BIT_USER set FirstName = '" + SelectedCoordinator.FirstName +
                "', LastName = '" + SelectedCoordinator.LastName +
                "', Street = '" + SelectedCoordinator.Street +
                "', Suburb = '" + SelectedCoordinator.Suburb +
                "', Postcode = '" + SelectedCoordinator.Postcode +
                "', State = '" + SelectedCoordinator.State +
                "', Email = '" + SelectedCoordinator.Email +
                "', Phone = '" + SelectedCoordinator.Phone +
                "', DateOfBirth = '" + Convert.ToDateTime(SelectedCoordinator.DOB).ToString("yyyy-MM-dd") +
                "', Password = '" + SelectedCoordinator.Password +
                "' where UserId = " + SelectedCoordinator.CoordinatorId;

            SQLHelper objHelper = new SQLHelper("BIT");
            objHelper.ExecuteNonQuery(sqlStr);
        }
    }
}
