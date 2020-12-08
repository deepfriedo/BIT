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
    public class ClientViewModel : ObservableObject
    {
        private ObservableCollection<Client> _clients;
        private Client _selectedClient;

        public ObservableCollection<Client> Clients
        {
            get { return _clients; }
            set { _clients = value; OnPropertyChanged("Clients"); }
        }
        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { _selectedClient = value; OnPropertyChanged("SelectedClient"); }
        }

        public ClientViewModel()
        {
            FileLogger fileLogger = new FileLogger();
            fileLogger.Log(" Entering ClientViewModel");

            GetAllClients();
        }

        public virtual ObservableCollection<Client> GetAllClients()
        {
            Clients allClients = new Clients();
            Clients = new ObservableCollection<Client>(allClients);
            return Clients;
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
            Clients allClients = new Clients();
            Clients = new ObservableCollection<Client>(allClients);

            FileLogger fileLogger = new FileLogger();
            fileLogger.Log(" Client's Cancel is pressed");
        }

        public void UpdateMethod()
        {
            string sqlStr = "update BIT_USER set FirstName = '" + SelectedClient.FirstName +
                "', LastName = '" + SelectedClient.LastName +
                "', Street = '" + SelectedClient.Street +
                "', Suburb = '" + SelectedClient.Suburb +
                "', Postcode = '" + SelectedClient.Postcode +
                "', State = '" + SelectedClient.State +
                "', Email = '" + SelectedClient.Email +
                "', Phone = '" + SelectedClient.Phone +
                "', DateOfBirth = '" + Convert.ToDateTime(SelectedClient.DOB).ToString("yyyy-MM-dd") +
                "', Password = '" + SelectedClient.Password +
                "' where UserId = " + SelectedClient.ClientId;

            SQLHelper objHelper = new SQLHelper("BIT");
            objHelper.ExecuteNonQuery(sqlStr);

            FileLogger fileLogger = new FileLogger();
            fileLogger.Log(" Client's Update is pressed");
        }
    }
}
