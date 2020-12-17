using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class Coordinator : BaseUser
    {
        private int _coordinatorId;
        private SQLHelper _db;

        public int CoordinatorId
        {
            get { return _coordinatorId; }
            set { _coordinatorId = value; }
        }

        public Coordinator()
        {
            _db = new SQLHelper("BIT");
        }

        public Coordinator(DataRow dr)
        {
            CoordinatorId = Convert.ToInt32(dr["CoordinatorId"]);
            FirstName = dr["firstname"].ToString();
            LastName = dr["lastname"].ToString();
            Street = dr["street"].ToString();
            Suburb = dr["suburb"].ToString();
            Postcode = dr["postcode"].ToString();
            State = dr["state"].ToString();
            Email = dr["email"].ToString();
            Phone = dr["phone"].ToString();
            DOB = Convert.ToDateTime(dr["dateofbirth"]);
            Password = dr["password"].ToString();
            _db = new SQLHelper("BIT");
        }
    }
}
