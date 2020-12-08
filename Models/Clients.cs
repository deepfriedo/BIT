using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBITProject.Data_Access_Layer;

namespace WpfBITProject.Models
{
    public class Clients : List<Client>
    {
        public Clients()
        {
            SQLHelper db = new SQLHelper("BIT");
            string sql = "select c.ClientId, firstname, lastname, street, suburb, postcode, state, email, phone, dateofbirth, password from BIT_USER b, CLIENT c where c.ClientId = b.UserId";
            DataTable clientsTable = db.ExecuteSQL(sql);

            foreach (DataRow dr in clientsTable.Rows)
            {
                Client customer = new Client(dr);
                this.Add(customer);
            }
        }
    }
}
