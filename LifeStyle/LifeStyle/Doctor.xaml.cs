﻿using LifeStyle.Extensions;
using LifeStyle.WindowSwitcher;
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
using System.Windows.Shapes;
using LifeStyle.DataBase;


namespace LifeStyle
{
    /// <summary>
    /// Логика взаимодействия для Doctor.xaml
    /// </summary>
    public partial class Doctor : Window
    {
        public Doctor()
        {
            InitializeComponent();
            //WindowHelper.SetWindowBackground(this, "services_list_background.jpg");
            //WindowHelper.SetPanelBackground((DoctorPanel.Items[0] as TabItem).Content as Grid, "services_list_background.jpg");

        }

       

        private void RedSchedule_Click(object sender, RoutedEventArgs e)
        {

        }

       
        private void cabinet_number_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cab_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PriemKn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Switcher.SwitchWindow(this, new LogIn());
        }

        private void EditECP_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditVisits_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
