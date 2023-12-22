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
using LifeStyle.Extensions;
using LifeStyle.WindowSwitcher;
using LifeStyle.DataBase;
using LifeStyle.ProjectFiles.ProjectEntities;
using LifeStyle.Paths;
using LifeStyle.ProjectFiles.Extensions;

namespace LifeStyle
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        private ProjectFiles.ProjectEntities.Client _Client;
        private IDictionary<string, ProjectFiles.ProjectEntities.Doctor> _Doctors;
        private List<ProjectFiles.ProjectEntities.ServiceInformation> _Services;
        public Client()
        {
            InitializeComponent();
            InitializeWindowComponent();
            this.Loaded += Client_Loaded;
        }

        private void Client_Loaded(object sender, RoutedEventArgs e)
        {
            UserProfileImage.Source = new BitmapImage(new Uri(System.IO.Path.Combine(PathWorker.Icon, "default_user_profile_image.jpg")));
            UserPhoto.Background = new ImageBrush(UserProfileImage.Source.Clone());
            UserName.Content = _Client.FullName;
            NotificationsIcon.Source = new BitmapImage(new Uri(System.IO.Path.Combine(PathWorker.Icon, "notification_icon.png")));
            CloseNotifications.Source = new BitmapImage(new Uri(System.IO.Path.Combine(PathWorker.Icon, "close_icon.png")));
            CloseDocuments.Source = CloseNotifications.Source.Clone();
            LoadDoctors();
            LoadServices();
        }

        public Client(Entitiy client): this()
        {
            _Client = client.Clone() as ProjectFiles.ProjectEntities.Client;
        }

        public void SetPanelBackground(ICollection<string> controlsName, ICollection<string> backgroundFile)
        {
            for(int i = 0;i < controlsName.Count;i++)
            {
                SetPanelBackground(controlsName.ElementAt(i), backgroundFile.ElementAt(i));
            }
        }

        private void SetPanelBackground(string panelName,string filename)
        {
            foreach(TabItem item in UserNavigationPanel.Items)
            {
                if(item.Header.ToString() == panelName)
                {
                    WindowHelper.SetPanelBackground(item.Content as Grid, filename);
                    break;
                }
            }
        }

        private void LoadDoctors()
        {
            var doctors = DBHelper.DbWorker.ExecuteFromDBCommand(
                $"SELECT * FROM Doctor;"
                );

            _Doctors = new Dictionary<string, ProjectFiles.ProjectEntities.Doctor>();

            for (int i = 0; i < doctors.Rows.Count; i++)
            {
                var doctor = (ProfileHelper.InitializeDoctor(doctors, i) as ProjectFiles.ProjectEntities.Doctor);

                _Doctors.Add(doctor.Email, doctor);

                DoctorListPlace.Children.Add(WindowHelper.CreateDoctorCard(doctor));
            }

        }

        private void LoadServices()
        {
            var services = DBHelper.DbWorker.ExecuteFromDBCommand(
                $"SELECT * FROM services"
                );

            _Services = new List<ServiceInformation>();

            for (int i = 0; i < services.Rows.Count; i++)
            {
                var service = WindowHelper.LoadServices(services, i);

                _Services.Add(service);
                ServiceListPlace.Children.Add(WindowHelper.CreateServiceCard(_Client,service));
            }
        }

        private void InitializeWindowComponent()
        {
            List<string> controlsName = new List<string>();

            foreach(TabItem item in UserNavigationPanel.Items)
            {
                controlsName.Add(item.Header.ToString());
            }

            SetPanelBackground(controlsName, new[] { "default_fon.jpg", "doctor_list_background.jpeg", "services_list_background.jpg", "visit_list_background.jpg" });
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.SwitchWindow(this, new LogIn());
        }

        private void NotificationsIcon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NotificationsContainer.Visibility = Visibility.Visible;
            NotificationsContainer.IsEnabled = true;
        }

        private void CloseNotifications_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NotificationsContainer.Visibility = Visibility.Hidden;
            NotificationsContainer.IsEnabled = false;
        }

        private void CloseDocuments_MouseUp(object sender, MouseButtonEventArgs e)
        {
            UserDocumentsPlace.Visibility = Visibility.Hidden;
            UserDocumentsPlace.IsEnabled = false;
        }

        private void EditProfileButton_Click(object sender, RoutedEventArgs e)
        {
            foreach(UIElement item in UserProfileInformationPanel.Children)
            {
                if(item is StackPanel panel)
                {
                    foreach(var innerItem in panel.Children)
                    {
                        if(innerItem is TextBox box)
                        {
                            box.IsReadOnly = false;
                        }
                    }
                }
            }

            EditProfileButton.IsEnabled = false;
            EditProfileButton.Visibility = Visibility.Hidden;

            SaveProfileButton.IsEnabled = true;
            SaveProfileButton.Visibility = Visibility.Visible;
        }

        private void UserDocuments_Click(object sender, RoutedEventArgs e)
        {
            UserDocumentsPlace.Visibility = Visibility.Visible;
            UserDocumentsPlace.IsEnabled = true;
        }

        private void SaveProfileButton_Click(object sender, RoutedEventArgs e)
        {

            foreach (UIElement item in UserProfileInformationPanel.Children)
            {
                if (item is StackPanel panel)
                {
                    foreach (var innerItem in panel.Children)
                    {
                        if (innerItem is TextBox box)
                        {
                            box.IsReadOnly = true;
                        }
                    }
                }
            }

            SaveProfileButton.IsEnabled = false;
            SaveProfileButton.Visibility = Visibility.Hidden;

            EditProfileButton.IsEnabled = true;
            EditProfileButton.Visibility = Visibility.Visible;
        }

        private void UserProfileImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!SaveProfileButton.IsEnabled)
                return;

            var fileDialog = new Microsoft.Win32.OpenFileDialog();

            fileDialog.Filter = "Image files |*.png; *.jpg; *.svg";

            bool result = (bool)fileDialog.ShowDialog();

            if(result)
            {
                UserProfileImage.Source = new BitmapImage(new Uri(fileDialog.FileName));
                UserPhoto.Background = new ImageBrush(UserProfileImage.Source);
            }

        }

        private void CreateDoctor_Click(object sender, RoutedEventArgs e)
        {
            DoctorListPlace.Children.Add(WindowHelper.CreateDoctorCard(this));
        }

        private void CreateService_Click(object sender, RoutedEventArgs e)
        {
            ServiceListPlace.Children.Add(WindowHelper.CreateServiceCard(this));
        }

        private void CreateVisit_Click(object sender, RoutedEventArgs e)
        {
            VisitsListPlace.Children.Add(WindowHelper.CreateVisitCard(this));
        }
    }
}
