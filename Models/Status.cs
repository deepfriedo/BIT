using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class Status : ObservableObject
    {
        private string _statusName;
        private SQLHelper _db;

        public string StatusName
        {
            get { return _statusName; }
            set { _statusName = value; OnPropertyChanged("StatusName"); }
        }
        public Status()
        {
            _db = new SQLHelper("BIT");
        }
        public Status(DataRow dr)
        {
            StatusName = dr["StatusName"].ToString();
        }
    }
}
