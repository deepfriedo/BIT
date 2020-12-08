using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class PreferredSuburb : ObservableObject
    {
        public string Suburb { get; set; }
        private SQLHelper _db;

        public PreferredSuburb()
        {
            _db = new SQLHelper("BIT");
        }

        public PreferredSuburb(DataRow dr)
        {
            Suburb = dr["Suburb"].ToString();
        }
    }
}
