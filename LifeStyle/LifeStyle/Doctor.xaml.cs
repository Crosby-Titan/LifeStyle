using LifeStyle.Extensions;
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
using LifeStyle.ProjectFiles.ProjectEntities;
using LifeStyle.Paths;
using LifeStyle.ProjectFiles.Extensions;

namespace LifeStyle
{
    /// <summary>
    /// Логика взаимодействия для Doctor.xaml
    /// </summary>
    public partial class Doctor : Window
    {
        private ProjectFiles.ProjectEntities.Doctor _Doctor;
        public Doctor()
        {
            InitializeComponent();
            this.Loaded += Doctor_Loaded;
        }

        public Doctor(Entitiy doctor): this()
        {
            _Doctor = doctor.Clone() as ProjectFiles.ProjectEntities.Doctor;
        }

        private void RedSchedule_Click(object sender, RoutedEventArgs e)
        {

        }

       
        private void cabinet_number_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cab_Click(object sender, RoutedEventArgs e)
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

        private void Doctor_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeImages();
            InitializeDoctorProfile();
            LoadECP();
            LoadVisits();
        }

        private void InitializeDoctorProfile()
        {
            var fullname = _Doctor.FullName.Split(' ');

            DoctorFirstName.Text = fullname[0];
            DoctorSecondName.Text = fullname[1];
            DoctorThirdName.Text = fullname[2];
            DoctorName.Content = _Doctor.FullName;
            DoctorSpecialization.Text = _Doctor.Specialization;
            DoctorCabinet.Text = _Doctor.CabinetNumber;
        }

        private void LoadVisits()
        {
            var visits = DBHelper.DbWorker.ExecuteFromDBCommand(
                $"SELECT id_reseption,fullname_patient,date_and_time,user_message,cabinet_number " +
                $"FROM Reseption;"
                );

            var rowsList = new List<TableVisitInfo>();

            for (int i = 0; i < visits.Rows.Count; i++)
            {
                rowsList.Add(WindowHelper.LoadDoctorVisits(visits,i));
            }

            VisitsTable.ItemsSource = rowsList;

            CurrentVisits.Text = $"{rowsList[0].VisitID} | {rowsList[0].fullNamePatient} | {rowsList[0].dateOfVisit}";
        }

        private void LoadECP()
        {
            var visits = DBHelper.DbWorker.ExecuteFromDBCommand(
                $"SELECT id_ECP, fullname_patient, date_and_time ," +
                $"Analysis , fullname_doctor ,Result_ " +
                $"FROM ECP;  "
                );

            for (int i = 0; i < visits.Rows.Count; i++)
            {
                VisitsTable.Items.Add(visits.Rows[i]);
            }
        }

        private void InitializeImages()
        {
            DoctorProfileImage.Source = new BitmapImage(new Uri(System.IO.Path.Combine(PathWorker.Icon, "default_user_profile_image.jpg")));
            DoctorPhoto.Background = new ImageBrush(DoctorProfileImage.Source.Clone());

            WindowHelper.SetPanelBackground(DoctorPanel,
                WindowHelper.GetControlsName(DoctorPanel),
                new[] { "services_list_background.jpg", "doctor_list_background.jpg", "visit_list_background.jpg" });
        }
    }
}
