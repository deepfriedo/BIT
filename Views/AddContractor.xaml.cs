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
using WpfBITProject.ViewModels;

namespace WpfBITProject.Views
{
    /// <summary>
    /// Interaction logic for AddContractor.xaml
    /// </summary>
    public partial class AddContractor : Page
    {
        public AddContractor()
        {
            InitializeComponent();
            this.DataContext = new AddContractorViewModel();
        }
    }
}
