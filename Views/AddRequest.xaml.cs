﻿using System;
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
    /// Interaction logic for AddRequest.xaml
    /// </summary>
    public partial class AddRequest : Page
    {
        public AddRequest()
        {
            InitializeComponent();
            this.DataContext = new AddRequestViewModel();
        }
    }
}
