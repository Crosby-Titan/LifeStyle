using LifeStyle.ProjectFiles.ProjectEntities;
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
            notify.Invoke(new VisitInformation
            {
                dateOfVisit = new DateTime(VisitDate.SelectedDate.Value.Ticks),
                Description = UserMessage.Text,
                ServiceName = _Service.ServiceName,
                fullNameDoctor = _DoctorName,
                fullNamePatient = _Client.FullName
            }); ;
        }
    }
}
