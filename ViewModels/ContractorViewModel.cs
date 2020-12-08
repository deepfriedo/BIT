using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using WpfBITProject.Models;
using WpfBITProject.Data_Access_Layer;
using NLog;
using System.Windows;

namespace WpfBITProject.ViewModels
{
    public class ContractorViewModel : ObservableObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private ObservableCollection<Contractor> _contractors;
        private Contractor _selectedContractor;

        public ObservableCollection<Contractor> Contractors
        {
            get { return _contractors; }
            set { _contractors = value; OnPropertyChanged("Contractors"); }
        }
        public Contractor SelectedContractor
        {
            get { return _selectedContractor; }
            set { _selectedContractor = value; OnPropertyChanged("SelectedContractor"); }
        }

        private ObservableCollection<Active> _actives;
        private Active _selectedActive;

        public ObservableCollection<Active> Actives
        {
            get { return _actives; }
            set { _actives = value; OnPropertyChanged("Actives"); }
        }
        public Active SelectedActive
        {
            get { return _selectedActive; }
            set { _selectedActive = value; OnPropertyChanged("SelectedActive"); }
        }

        public ContractorViewModel()
        {
            logger.Info("Entering the ContractorViewModel.");

            LoadActives();
            GetAllContractors();
        }

        public virtual ObservableCollection<Contractor> GetAllContractors()
        {
            Contractors allContractors = new Contractors();
            Contractors = new ObservableCollection<Contractor>(allContractors);
            return Contractors;
        }

        public void LoadActives()
        {
            Actives allActives = new Actives();
            Actives = new ObservableCollection<Active>(allActives);
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

        public void UpdateMethod()
        {
            string sqlStr = "update BIT_USER set FirstName = '" + SelectedContractor.FirstName +
                "', LastName = '" + SelectedContractor.LastName +
                "', Street = '" + SelectedContractor.Street +
                "', Suburb = '" + SelectedContractor.Suburb +
                "', Postcode = '" + SelectedContractor.Postcode +
                "', State = '" + SelectedContractor.State +
                "', Email = '" + SelectedContractor.Email +
                "', Phone = '" + SelectedContractor.Phone +
                "', DateOfBirth = '" + Convert.ToDateTime(SelectedContractor.DOB).ToString("yyyy-MM-dd") +
                "', Password = '" + SelectedContractor.Password +
                "' where UserId = " + SelectedContractor.ContractorId +
                " update CONTRACTOR set Active = '" + SelectedActive.ActiveName +
                "' where ContractorId = " + SelectedContractor.ContractorId;

            SQLHelper objHelper = new SQLHelper("BIT");
            objHelper.ExecuteNonQuery(sqlStr);

            logger.Info("Contractor's Update is pressed");
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
            Contractors allContractors = new Contractors();
            Contractors = new ObservableCollection<Contractor>(allContractors);

            logger.Info("Contractor's Cancel is pressed");
        }

        private MyCommand _findActiveCommand;
        public MyCommand FindActiveCommand
        {
            get
            {
                if (_findActiveCommand == null)
                {
                    _findActiveCommand = new MyCommand(this.FindActiveMethod, true);
                }
                return _findActiveCommand;
            }
            set
            {
                _findActiveCommand = value;
            }
        }

        private MyCommand _findInactiveCommand;
        public MyCommand FindInactiveCommand
        {
            get
            {
                if (_findInactiveCommand == null)
                {
                    _findInactiveCommand = new MyCommand(this.FindInactiveMethod, true);
                }
                return _findInactiveCommand;
            }
            set
            {
                _findInactiveCommand = value;
            }
        }

        public void FindActiveMethod()
        {
            ActiveContractors allActiveContractors = new ActiveContractors();
            Contractors = new ObservableCollection<Contractor>(allActiveContractors);

            logger.Info("Contractor's Active is pressed");
        }

        public void FindInactiveMethod()
        {
            InactiveContractors allInactiveContractors = new InactiveContractors();
            Contractors = new ObservableCollection<Contractor>(allInactiveContractors);

            logger.Info("Contractors' Inactive is pressed");
        }
    }
}
