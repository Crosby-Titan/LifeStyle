using LifeStyle.ProjectFiles.ProjectEntities;
using Microsoft.VisualBasic;
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

namespace LifeStyle
{
    /// <summary>
    /// Логика взаимодействия для SelectedService.xaml
    /// </summary>
    public partial class SelectedService : Window
    {
        public delegate void NotifyParent(VisitInformation visit);
        public event NotifyParent notify;
        private ProjectFiles.ProjectEntities.Client _Client;
        private ServiceInformation _Service;
        private string _DoctorName;
        public SelectedService()
        {
            InitializeComponent();
            this.Loaded += SelectedService_Loaded;
        }

        private void SelectedService_Loaded(object sender, RoutedEventArgs e)
        {
            string[] availableTimeArray = new[]
            {
                "8:00",
                "9:00",
                "10:00",
                "11:00",
                "12:00",
                "13:00",
                "14:00",
                "15:00",
                "16:00",
                "17:00",
                "18:00",
                "19:00",
                "20:00",
                "21:00",
                "22:00",
            };
            VisitTime.ItemsSource = availableTimeArray;
        }

        public SelectedService(string doctorName, ProjectFiles.ProjectEntities.Client client, ServiceInformation service) : this()
        {
            this.Title = service.ServiceName;
            _Client = client;
            _Service = service;
            _DoctorName = doctorName;
        }

        private void SaveVisit_Click(object sender, RoutedEventArgs e)
        {
            var date = new DateTime(VisitDate.SelectedDate.Value.Ticks);
            date = date.AddHours(DateTime.Parse(VisitTime.SelectedValue.ToString()).Hour);
            notify.Invoke(new VisitInformation
            {
                dateOfVisit = date,
                Description = UserMessage.Text,
                ServiceName = _Service.ServiceName,
                fullNameDoctor = _DoctorName,
                fullNamePatient = _Client.FullName
            });
            this.Close();
        }
    }
}
