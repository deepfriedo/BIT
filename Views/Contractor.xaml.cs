using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfBITProject.Models;
using WpfBITProject.ViewModels;

namespace WpfBITProject.Views
{
    /// <summary>
    /// Interaction logic for Contractor.xaml
    /// </summary>
    public partial class Contractor : Page
    {
        public Contractor()
        {
            InitializeComponent();

            this.DataContext = new ContractorViewModel();
        }

        private void btnAddNewContractor_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Views/AddContractor.xaml", UriKind.Relative));
        }
    }
}
