using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Models;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.ViewModels
{
    public class AddClientViewModel : ObservableObject
    {
        private Client _client;
        public Client Client
        {
            get { return _client; }
            set { _client = value; OnPropertyChanged("Client"); }
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
            Client = new Client();
        }

        public void AddMethod()
        {
            string sqlStr = "DECLARE @NewPrimaryKeyClient INT INSERT INTO BIT_USER(FirstName, LastName, Street, Suburb, Postcode, State, Email, Phone, DateOfBirth, Password) VALUES('" + Client.FirstName + "', '" + Client.LastName + "', '" + Client.Street + "', '" + Client.Suburb + "', '" + Client.Postcode + "', '" + Client.State + "', '" + Client.Email + "', '" + Client.Phone + "', '" + Convert.ToDateTime(Client.DOB).ToString("yyyy-MM-dd") + "', '" + Client.Password + "') SET @NewPrimaryKeyClient = @@IDENTITY INSERT INTO CLIENT(ClientId) VALUES(@NewPrimaryKeyClient)";

            SQLHelper objHelper = new SQLHelper("BIT");
            objHelper.ExecuteNonQuery(sqlStr);
        }

        public AddClientViewModel()
        {
            Client = new Client();
        }
    }
}
