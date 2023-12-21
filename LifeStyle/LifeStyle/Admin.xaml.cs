using System;
using System.Collections.Generic;
using System.Data;
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
using LifeStyle.Extensions;
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
        private List<object[]> _Requests;
        public Admin()
        {
            InitializeComponent();
            InitializeUIElements();
        }

        public Admin(Entitiy admin): this()
        {
            _Admin = admin.Clone() as ProjectFiles.ProjectEntities.Admin;
            _AdminPanel = new AdminPanel(_Admin);
        }

        private void InitializeUIElements()
        {
            LoadRequests();
            LoadECPdata();
        }

        private void LoadECPdata()
        {
            var ecpData = DBHelper.DbWorker.ExecuteFromDBCommand($"SELECT * FROM ecp;");

            foreach(DataRow dataRow in ecpData.Rows)
            {
                ReadOnlyECPList.Items.Add(dataRow.ItemArray);
            }
        }

        private void LoadRequests()
        {
            _Requests = WindowHelper.LoadRegistrationRequests();
            foreach (var item in _Requests)
            {
                UsersRegistrationRequestList.Children.Add(WindowHelper.CreateRegistrationRequest(item));
            }
        }

        private void Allow_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequests = SearchSelectedRequests(UsersRegistrationRequestList);

            foreach(UIElement allowedUser in selectedRequests)
            {
                SetStatus(((allowedUser as StackPanel).Children[1] as Label).Content.ToString(), UserStatus.Actual);
                UsersRegistrationRequestList.Children.Remove(allowedUser);
            }
        }

        private List<UIElement> SearchSelectedRequests(UIElement element)
        {
            var panel = element as StackPanel;
            var searchedElements = new List<UIElement>();

            foreach(var uiElement in panel.Children)
            {
                switch (uiElement)
                {
                    case StackPanel childPanel:
                        if (childPanel.Background is SolidColorBrush brush && brush.Color == (Color)ColorConverter.ConvertFromString("#EFB2D1"))
                            searchedElements.Add(childPanel);
                        break;
                    default:
                        break;
                }
            }

            return searchedElements;
        }
        private void SetStatus(string email,UserStatus status)
        {
            _AdminPanel.ChangeClientStatus(email, status);
        }

        private void Deny_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequests = SearchSelectedRequests(UsersRegistrationRequestList);

            foreach (UIElement allowedUser in selectedRequests)
            {
                SetStatus(((allowedUser as StackPanel).Children[1] as Label).Content.ToString(), UserStatus.Deleted);
                UsersRegistrationRequestList.Children.Remove(allowedUser);
            }
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
