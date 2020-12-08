using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class Active : ObservableObject
    {
        private string _activeName;
        private SQLHelper _db;

        public string ActiveName
        {
            get { return _activeName; }
            set { _activeName = value; OnPropertyChanged("ActiveName"); }
        }
        public Active()
        {
            _db = new SQLHelper("BIT");
        }
        public Active(DataRow dr)
        {
            ActiveName = dr["ActiveName"].ToString();
        }
    }
}
