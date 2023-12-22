using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
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
        private IDictionary<string, ProjectFiles.ProjectEntities.Client> _Clients;
        private IDictionary<string, ProjectFiles.ProjectEntities.Doctor> _Doctors;
        public Admin()
        {
            InitializeComponent();
            InitializeUIElements();
            this.Loaded += Admin_Loaded;
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
            LoadAllClients();
            LoadAllDoctors();
        }

        private void LoadECPdata()
        {
            var ecpData = DBHelper.DbWorker.ExecuteFromDBCommand($"SELECT * FROM ecp;");

            foreach(DataRow dataRow in ecpData.Rows)
            {
                ReadOnlyECPList.Items.Add(dataRow.ItemArray);
            }
        }
        
        public void LoadAllDoctors()
        {
            var doctors = DBHelper.DbWorker.ExecuteFromDBCommand(
                $"SELECT * FROM Doctor;"
                );

            _Doctors = new Dictionary<string, ProjectFiles.ProjectEntities.Doctor>();

            for (int i = 0;i < doctors.Rows.Count;i++)
            {
                var doctor = (ProfileHelper.InitializeDoctor(doctors, i) as ProjectFiles.ProjectEntities.Doctor);

                if(!_Doctors.ContainsKey(doctor.Email))
                    _Doctors.Add(doctor.Email, doctor);

                DoctorList.Items.Add(doctor.Email);
            }
        }

        public void LoadAllClients()
        {
            var clients = DBHelper.DbWorker.ExecuteFromDBCommand(
                $"SELECT * FROM patient_personal_account,Passport;"
                );

            _Clients = new Dictionary<string, ProjectFiles.ProjectEntities.Client>();

            for (int i = 0; i < clients.Rows.Count; i++)
            {
                var client = (ProfileHelper.InitializeClient(clients, i) as ProjectFiles.ProjectEntities.Client);
                if(!_Clients.ContainsKey(client.Email))
                    _Clients.Add(client.Email, client);
                PatientList.Items.Add(client.Email);
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


        public void SetPanelBackground(ICollection<string> controlsName, ICollection<string> backgroundFile)
        {
            for (int i = 0; i < controlsName.Count; i++)
            {
                SetPanelBackground(controlsName.ElementAt(i), backgroundFile.ElementAt(i));
            }
        }

        private void SetPanelBackground(string panelName, string filename)
        {
            foreach (TabItem item in AdminPanel.Items)
            {
                if (item.Header.ToString() == panelName)
                {
                    WindowHelper.SetPanelBackground(item.Content as Grid, filename);
                    break;
                }
            }
        }

        private void Deny_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequests = SearchSelectedRequests(UsersRegistrationRequestList);

            foreach (UIElement deniedRequest in selectedRequests)
            {
                SetStatus(((deniedRequest as StackPanel).Children[1] as Label).Content.ToString(), UserStatus.Deleted);
                UsersRegistrationRequestList.Children.Remove(deniedRequest);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            WindowSwitcher.Switcher.SwitchWindow(this, new LogIn());
        }

        private void EditDoctors_Click(object sender, RoutedEventArgs e)
        {
            if(DoctorList.SelectedIndex <  0) return;   

            var doctor = _Doctors[DoctorList.SelectedItem.ToString()];

            DoctorLogin.Text = doctor.Email;
            string[] fullName = doctor.FullName.Split(' ');
            DoctorFirstName.Text = fullName[0];
            DoctorSecondName.Text = fullName[1];
            DoctorThirdName.Text = fullName[2];
            DoctorSpecialization.Text = doctor.Specialization;
        }

        private void SaveDoctor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditUsers_Click(object sender, RoutedEventArgs e)
        {
            if (PatientList.SelectedIndex < 0) return;

            var client = _Clients[PatientList.SelectedItem.ToString()];

            PatientLogin.Text = client.Email;
            string[] fullName = client.FullName.Split(' ');
            PatientFirstName.Text = fullName[0];
            PatientSecondName.Text = fullName[1];
            PatientThirdName.Text = fullName[2];
            PatientBirthDay.SelectedDate = new DateTime(client.BirthDay.Ticks);
        }

        private void RegDoctor_Click(object sender, RoutedEventArgs e)
        {
            if(ProfileHelper.Exists(DoctorLogin.Text))
            {
                WindowHelper.ShowErrorMessageBox($"Пользователь с логином {PatientLogin.Text} " +
                    $"уже существует!");
                return;
            }

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
            if (ProfileHelper.Exists(PatientLogin.Text))
            {
                WindowHelper.ShowErrorMessageBox($"Пользователь с логином {PatientLogin.Text} " +
                    $"уже существует!");
                return;
            }

            ProfileHelper.RegistrationClient(new ClientRegistrationInfo
            {
                Email = PatientLogin.Text,
                BirthDay = new DateTime(PatientBirthDay.SelectedDate.Value.Ticks),
                FullName = new[] { PatientFirstName.Text, PatientSecondName.Text, PatientThirdName.Text },
                PasswordHashCode = PatientPassword.Text.GetHashCode(),
            });
        }

        private List<string> GetControlsName(ItemsControl control)
        {
            List<string> controlsName = new List<string>();

            foreach (TabItem item in control.Items)
            {
                controlsName.Add(item.Header.ToString());
            }

            return controlsName;
        }

        private void Admin_Loaded(object sender, RoutedEventArgs e)
        {
            AdminPhoto.Background = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine(Paths.PathWorker.Icon, "admin_default_image_profile.jpg"))));

            SetPanelBackground(GetControlsName(AdminPanel), new[] { "services_list_background.jpg", "services_list_background.jpg", "services_list_background.jpg", "doctor_list_background.jpg", "doctor_list_background.jpg" });
        }

    }
}
