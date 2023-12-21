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
using LifeStyle.ProjectFiles.Extensions;
using LifeStyle.ProjectFiles.ProjectEntities;

namespace LifeStyle
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        private ProjectFiles.ProjectEntities.Admin _Admin;
        private AdminPanel _AdminPanel;
        public Admin()
        {
            InitializeComponent();
        }

        public Admin(Entitiy admin): this()
        {
            _Admin = admin.Clone() as ProjectFiles.ProjectEntities.Admin;
            _AdminPanel = new AdminPanel(_Admin);
        }

        private void Allow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Deny_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            WindowSwitcher.Switcher.SwitchWindow(this, new LogIn());
        }

        private void EditDoctors_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditUsers_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RegDoctor_Click(object sender, RoutedEventArgs e)
        {
            ProfileHelper.RegistrationDoctor(new DoctorRegistrationInfo
            {
                Email = DoctorLogin.Text,
                FullName = new[] { DoctorFirstName.Text, DoctorSecondName.Text, DoctorThirdName.Text },
                Password = DoctorPassword.Text,
                Specialization = DoctorSpecialization.Text
            });
        }

        private void RegUser_Click(object sender, RoutedEventArgs e)
        {
            ProfileHelper.RegistrationClient(new ClientRegistrationInfo
            {
                Email = PatientLogin.Text,
                BirthDay = new DateTime(PatientBirthDay.SelectedDate.Value.Ticks),
                FullName = new[] { PatientFirstName.Text, PatientSecondName.Text, PatientThirdName.Text },
                PasswordHashCode = PatientPassword.Text.GetHashCode(),
            });
        }
    }
}
