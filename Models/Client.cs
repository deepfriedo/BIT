using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using WpfBITProject.Data_Access_Layer;
using NLog;

namespace WpfBITProject.Models
{
    public class Client : BaseUser
    {
        private int _clientId;
        private SQLHelper _db;

        public int ClientId
        {
            get { return _clientId; }
            set { _clientId = value; }
        }

        public Client()
        {
            _db = new SQLHelper("BIT");
        }

        public Client(DataRow dr)
        {
            ClientId = Convert.ToInt32(dr["clientid"]);
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
