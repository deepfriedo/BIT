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
using WpfBITProject.Views;

namespace WpfBITProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnClient_Click(object sender, RoutedEventArgs e)
        {
            contentFrame.Content = new Views.Client();
        }

        private void btnContractor_Click(object sender, RoutedEventArgs e)
        {
            contentFrame.Content = new Views.Contractor();
        }

        private void btnRequest_Click(object sender, RoutedEventArgs e)
        {
            contentFrame.Content = new Views.Request();
        }

        private void btnSchedule_Click(object sender, RoutedEventArgs e)
        {
            contentFrame.Content = new Views.Schedule();
        }

        private void btnCoordinator_Click(object sender, RoutedEventArgs e)
        {
            contentFrame.Content = new Views.Coordinator();
        }

        private void btnSkill_Click(object sender, RoutedEventArgs e)
        {
            contentFrame.Content = new Views.AddSkill();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
