using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfBITProject.Data_Access_Layer;
using WpfBITProject.Models;

namespace WpfBITProject.ViewModels
{
    public class LoginViewModel
    {
        public Coordinator Coordinator { get; set; }

        public LoginViewModel()
        {
            Coordinator = new Coordinator();
        }

        private MyCommand _loginCommand;
        public MyCommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    _loginCommand = new MyCommand(this.LoginMethod, true);
                }
                return _loginCommand;
            }
            set
            {
                _loginCommand = value;
            }
        }

        public void LoginMethod()
        {
            string sqlStr = "SELECT COUNT(1) FROM BIT_USER b, COORDINATOR c WHERE b.UserId = c.CoordinatorId AND Email = '" + Coordinator.Email + "' AND Password = '" + Coordinator.Password + "'";

            SQLHelper objHelper = new SQLHelper("BIT");

            int count = Convert.ToInt32(objHelper.ExecuteSQLScalar(sqlStr));

            if (count == 1)
            {
                MainWindow mainwindow = new MainWindow();
                mainwindow.Show();
            }
            else
            {
                MessageBox.Show("Email or Password Incorrect");
            }
        }
    }
}
