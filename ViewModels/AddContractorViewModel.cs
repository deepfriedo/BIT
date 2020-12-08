using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Models;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.ViewModels
{
    public class AddContractorViewModel : ObservableObject
    {
        private Contractor _contractor;
        public Contractor Contractor
        {
            get { return _contractor; }
            set { _contractor = value; OnPropertyChanged("Contractor"); }
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
            Contractor = new Contractor();
        }

        public void AddMethod()
        {
            string sqlStr = "DECLARE @NewPrimaryKeyContractor INT INSERT INTO BIT_USER(FirstName, LastName, Street, Suburb, Postcode, State, Email, Phone, DateOfBirth, Password) VALUES('" + Contractor.FirstName + "', '" + Contractor.LastName + "', '" + Contractor.Street + "', '" + Contractor.Suburb + "', '" + Contractor.Postcode + "', '" + Contractor.State + "', '" + Contractor.Email + "', '" + Contractor.Phone + "', '" + Convert.ToDateTime(Contractor.DOB).ToString("yyyy-MM-dd") + "', '" + Contractor.Password + "') SET @NewPrimaryKeyContractor = @@IDENTITY INSERT INTO CONTRACTOR(ContractorId, Insurance, Active) VALUES(@NewPrimaryKeyContractor, 'PL', 'Available') INSERT INTO PREFERRED_SUBURB(ContractorId, Suburb, Postcode) VALUES(@NewPrimaryKeyContractor, '" + Contractor.Suburb + "', '" + Contractor.Postcode + "')";

            SQLHelper objHelper = new SQLHelper("BIT");
            objHelper.ExecuteNonQuery(sqlStr);
        }

        public AddContractorViewModel()
        {
            Contractor = new Contractor();
        }
    }
}
