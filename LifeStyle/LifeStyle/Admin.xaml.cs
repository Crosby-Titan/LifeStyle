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
using System.Windows.Shapes;
using LifeStyle.DataBase;

namespace LifeStyle
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        private ProjectFiles.ProjectEntities.Admin _Admin;
        public Admin()
        {
            InitializeComponent();
        }

        public Admin(ProjectFiles.ProjectEntities.Entitiy admin): this()
        {
            _Admin = admin.Clone() as ProjectFiles.ProjectEntities.Admin;
        }

        private void Allow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Deny_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
