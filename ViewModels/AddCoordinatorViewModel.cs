using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Models;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.ViewModels
{
    public class AddCoordinatorViewModel : ObservableObject
    {
        private Coordinator _coordinator;

        public Coordinator Coordinator
        {
            get { return _coordinator; }
            set { _coordinator = value; OnPropertyChanged("Coordinator"); }
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
            Coordinator = new Coordinator();
        }

        public void AddMethod()
        {
            string sqlStr = "DECLARE @NewPrimaryKeyCoordinator INT INSERT INTO BIT_USER(FirstName, LastName, Street, Suburb, Postcode, State, Email, Phone, DateOfBirth, Password) VALUES('" + Coordinator.FirstName + "', '" + Coordinator.LastName + "', '" + Coordinator.Street + "', '" + Coordinator.Suburb + "', '" + Coordinator.Postcode + "', '" + Coordinator.State + "', '" + Coordinator.Email + "', '" + Coordinator.Phone + "', '" + Convert.ToDateTime(Coordinator.DOB).ToString("yyyy-MM-dd") + "', '" + Coordinator.Password + "') SET @NewPrimaryKeyCoordinator = @@IDENTITY INSERT INTO COORDINATOR(CoordinatorId) VALUES(@NewPrimaryKeyCoordinator)";

            SQLHelper objHelper = new SQLHelper("BIT");
            objHelper.ExecuteNonQuery(sqlStr);
        }

        public AddCoordinatorViewModel()
        {
            Coordinator = new Coordinator();
        }
    }
}
