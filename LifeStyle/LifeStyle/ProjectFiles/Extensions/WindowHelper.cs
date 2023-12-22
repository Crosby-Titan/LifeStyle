using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LifeStyle.Paths;
using System.Windows.Controls;
using System.Windows.Markup;
using LifeStyle.ProjectFiles.ProjectEntities;
using System.IO;
using LifeStyle.DataBase;
using LifeStyle.ProjectFiles.Extensions;
using System.Data;

namespace LifeStyle.Extensions
{
    public static class WindowHelper
    {
        private static int _doctorCardCount = 0;
        public static void SetWindowBackground(Window window, string filename)
        {
            window.Background = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine(PathWorker.Background, filename))));
        }

        public static void SetControlBackground(Control control, string filename)
        {
            control.Background = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine(PathWorker.Background, filename))));
        }

        public static void SetPanelBackground(Panel panel, string filename)
        {
            panel.Background = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine(PathWorker.Background, filename))));
        }

        public static void ShowErrorMessageBox(string text)
        {
            MessageBox.Show(text, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static UIElement CreateDoctorCard(Window window)
        {
            XamlReader reader = new XamlReader();

            var xaml = reader.LoadAsync(new FileStream(@"D:\УП\LifeStyle\LifeStyle\XAML\DoctorCard.xaml", System.IO.FileMode.Open)) as ResourceDictionary;

            return xaml["DoctorCardTemplate"] as Border;
        }

        public static UIElement CreateServiceCard(Window window)
        {
            XamlReader reader = new XamlReader();

            var xaml = reader.LoadAsync(new FileStream(Path.Combine(PathWorker.XAML, "ServiceCard.xaml"), System.IO.FileMode.Open)) as ResourceDictionary;

            return xaml["ServiceCardTemplate"] as Border;
        }

        public static UIElement CreateVisitCard(Window window)
        {
            XamlReader reader = new XamlReader();

            var xaml = reader.LoadAsync(new FileStream(Path.Combine(PathWorker.XAML,"VisitCard.xaml"), System.IO.FileMode.Open)) as ResourceDictionary;

            return xaml["VisitCardTemplate"] as Border;
        }

        public static UIElement CreateVisitCard(VisitInformation visit)
        {
            XamlReader reader = new XamlReader();

            var xaml = reader.LoadAsync(new FileStream(Path.Combine(PathWorker.XAML,"VisitCard.xaml"), System.IO.FileMode.Open)) as ResourceDictionary;

            var visitCard = xaml["VisitCardTemplate"] as Border;

            foreach(UIElement element in (visitCard.Child as StackPanel).Children)
            {
                switch(element)
                {
                    case Label label:
                        label.Content = 
                            $"ФИО пациента: {visit.fullNamePatient}\n" +
                            $"Название услуги: {visit.ServiceName} ({visit.ServiceID})\n" +
                            $"Описание: {visit.Description}";
                        break;
                    case Button btn:
                        btn.Content = $"{visit.dateOfVisit.ToShortDateString()}/{visit.dateOfVisit.ToShortTimeString()}";
                        break;
                    case StackPanel panel:

                        foreach(UIElement innerElement  in panel.Children)
                        {
                            if (innerElement is TextBox box)
                            {
                                box.Text = visit.fullNameDoctor;
                            }
                            if(innerElement is Border border)
                            {
                                border.Background = new ImageBrush(new BitmapImage(visit.doctorPhoto ?? new Uri(Path.Combine(PathWorker.Icon, "template_user_profile_image.jpg"))));
                            }
                        }

                        break;
                    default:
                        break;
                }

            }

            return visitCard;
        }

        public static UIElement CreateServiceCard(ProjectFiles.ProjectEntities.Client client,ServiceInformation service)
        {
            XamlReader reader = new XamlReader();

            var xaml = reader.LoadAsync(new FileStream(Path.Combine(PathWorker.XAML, "ServiceCard.xaml"),FileMode.Open)) as ResourceDictionary;

            var serviceCard = xaml["ServiceCardTemplate"] as Border;

            string selectedDoctor = "";

            foreach(UIElement element in (serviceCard.Child as StackPanel).Children)
            {
                switch (element)
                {
                    case StackPanel panel:
                        
                        foreach(UIElement innerElement in panel.Children)
                        {
                            if(innerElement is Border doctorPhoto)
                            {
                                doctorPhoto.Background = new ImageBrush(
                                    new BitmapImage(
                                        new Uri(
                                            Path.Combine(PathWorker.Icon, "default_user_profile_image.jpg"))));
                            }
                            if(innerElement is ComboBox box)
                            {
                                var doctorsOnService = DBHelper.DbWorker.ExecuteFromDBCommand(
                                    $"SELECT fullname " +
                                    $"FROM Doctor " +
                                    $"WHERE Doctor.specialization = \'{service.Specialization}\';"
                                    );

                                for(int i = 0;i < doctorsOnService.Rows.Count;i++)
                                {
                                    box.Items.Add(doctorsOnService.Rows[i][0]);
                                }

                                box.SelectionChanged += (o, e) =>
                                {
                                    selectedDoctor = box.SelectedItem.ToString();
                                };
                            }
                        }

                        break;
                    case Label lbl:
                        lbl.Content = $" {service.ServiceName} | [{service.ServicePrice} р]";
                        break;
                    case Button btn:
                        btn.Click += (object sender, RoutedEventArgs e) =>
                        {
                            var selectedService = new SelectedService(selectedDoctor, client, service);

                            selectedService.notify += (info) =>
                            {
                                if(string.IsNullOrWhiteSpace(selectedDoctor))
                                {
                                    WindowHelper.ShowErrorMessageBox("Не выбран врач!");
                                    return;
                                }

                                DBHelper.DbWorker.ExecuteIntoDBCommand(
                                    $"INSERT INTO Reseption " +
                                    $"(fullname_patient,fullname_doctor,specialization_doctor,cabinet_number,date_and_time,Service,Patient_phone_number,SMPNumber,user_message) " +
                                    $"VALUES (" +
                                    $"\'{client.FullName}\'," +
                                    $"\'{selectedDoctor}\'," +
                                    $"(SELECT id_special FROM Spec WHERE spec = \'{service.Specialization}\' LIMIT 1)," +
                                    $"(SELECT cabinet_number FROM Doctor WHERE fullname = \'{selectedDoctor}\' LIMIT 1)," +
                                    $"\'{info.dateOfVisit}\'," +
                                    $"(SELECT id_services FROM Services WHERE service_list = \'{service.ServiceName}\' LIMIT 1)," +
                                    $"\'{client.Passport.PhoneNumber}\'," +
                                    $"\'{client.Passport.SMPNumber}\'," +
                                    $"\'{info.Description}\'" +
                                    $");"
                                    );
                            };

                            selectedService.Show();

                        };
                        break;
                    default:
                        break;
                }
            }

            return serviceCard;
        }

        public static UIElement CreateDoctorCard(ProjectFiles.ProjectEntities.Doctor doctor)
        {
            XamlReader reader = new XamlReader();

            var xaml = reader.LoadAsync(new FileStream(Path.Combine(PathWorker.XAML,"DoctorCard.xaml"),FileMode.Open)) as ResourceDictionary;

            var doctorCard = xaml["DoctorCardTemplate"] as Border;

            foreach (UIElement element in (doctorCard.Child as StackPanel).Children)
            {
                switch (element)
                {
                    case StackPanel panel:

                        foreach(UIElement innerElement in panel.Children)
                        {
                            switch (innerElement)
                            {
                                case Label lbl:
                                    if((lbl.Content as string).Contains("Врача",StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        lbl.Content = $"{doctor.FullName}";
                                    }
                                    break;
                                case StackPanel p:
                                    foreach(UIElement e in p.Children)
                                    {
                                        switch(e)
                                        {
                                            case Label s:
                                                if ((s.Content as string).Contains("Должность", StringComparison.CurrentCultureIgnoreCase))
                                                {
                                                    s.Content = $"\t{doctor.Specialization}\n\t{doctor.Experience} лет\n\tГрафик: {doctor.ShortWorkSchedule}";
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }

                        break;
                    case Button btn:
                        btn.Click += (object sender, RoutedEventArgs e) =>
                        {

                        };
                        break;
                    
                }

            }

            return doctorCard;
        }

        public static List<object[]> LoadRegistrationRequests()
        {
            var requests = DBHelper.DbWorker.ExecuteFromDBCommand(
                $"SELECT id_patient_personal_account,login,UserStatus.status " +
                $"FROM patient_personal_account " +
                $"INNER JOIN UserStatus " +
                $"ON patient_personal_account.status = UserStatus.ID " +
                $"WHERE " +
                $"UserStatus.status = \'{ProfileHelper.GetClientStatus(UserStatus.OnVerification)}\';"
                );

            var dataList = new List<object[]>();    

            foreach (DataRow request in requests.Rows)
            {
                dataList.Add(request.ItemArray);
            }

            return dataList;
        }

        public static VisitInformation LoadVisit(DataTable visits, int rowsLineNumber)
        {
            var visitsList = new Dictionary<string, string>();

            foreach (DataColumn column in visits.Columns)
            {
                switch (column.ColumnName)
                {
                    case "fullname_patient":
                    case "fullname_doctor":
                    case "cabinet_number":
                    case "date_and_time":
                    case "service":
                    case "service_list":
                    case "user_message":
                        visitsList.Add(column.ColumnName, visits.Rows[rowsLineNumber][column.Ordinal].ToString());
                        break;
                    default:
                        break;
                }

            }


            return new VisitInformation
            {
                 dateOfVisit = DateTime.Parse(visitsList["date_and_time"]),
                 fullNameDoctor = visitsList["fullname_doctor"],
                 fullNamePatient = visitsList["fullname_patient"],
                 ServiceID = int.Parse(visitsList["service"]),
                 ServiceName = visitsList["service_list"],
                 Description = $" Кабинет: {visitsList["cabinet_number"]}\nЖалоба: {visitsList["user_message"]}"
            };
        }

        public static UIElement CreateRegistrationRequest(object[] data)
        {

            var registrationRequest = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children =
                {
                    new Label
                    {
                        Content = data[0],
                        FontSize = 15d,
                        Foreground = new SolidColorBrush(Colors.White)
                    },
                    new Label
                    {
                        Content = data[1],
                        FontSize = 15d,
                        Foreground = new SolidColorBrush(Colors.White)
                    },
                    new Label
                    {
                        Content = data[2],
                        FontSize = 15d,
                        Foreground = new SolidColorBrush(Colors.White)
                    }
                },

            };

            registrationRequest.MouseUp += (o, e) =>
            {
                if(!(registrationRequest.Background is SolidColorBrush brush && brush.Color == (Color)ColorConverter.ConvertFromString("#EFB2D1")))
                {
                    registrationRequest.Background = new SolidColorBrush
                    {
                        Color = (Color)ColorConverter.ConvertFromString("#EFB2D1"),
                        Opacity = 0.5
                    };
                }
                else
                {
                    registrationRequest.Background = new SolidColorBrush(Colors.Transparent);
                }
            };

            return registrationRequest;
        }

        public static List<UIElement> CreateRegistrationRequests(List<object[]> data)
        {
            var uiList = new List<UIElement>();

            foreach(var item in data)
            {
                uiList.Add(CreateRegistrationRequest(item));
            }

            return uiList;
        }

        public static ServiceInformation LoadServices(DataTable services,int rowsLineNumber)
        {

            var serviceList = new Dictionary<string, string>();

            foreach(DataColumn column in services.Columns)
            {
                switch (column.ColumnName)
                {
                    case "specialization":
                    case "service_list":
                    case "price":
                        serviceList.Add(column.ColumnName, services.Rows[rowsLineNumber][column.Ordinal].ToString());
                        break;
                    default:
                        break;
                }

            }


            return new ServiceInformation
            {
                Description = "",
                ServiceName = serviceList["service_list"],
                ServicePrice = decimal.Parse(serviceList["price"]),
                Specialization = serviceList["specialization"]
            };
        }

        public static void SetPanelBackground(ItemsControl panel,ICollection<string> controlsName, ICollection<string> backgroundFile)
        {
            for (int i = 0; i < controlsName.Count; i++)
            {
                SetPanelBackground(panel,controlsName.ElementAt(i), backgroundFile.ElementAt(i));
            }
        }

        public static void SetPanelBackground(ItemsControl panel,string panelName, string filename)
        {
            foreach (TabItem item in panel.Items)
            {
                if (item.Header.ToString() == panelName)
                {
                    WindowHelper.SetPanelBackground(item.Content as Grid, filename);
                    break;
                }
            }
        }

        public static List<string> GetControlsName(ItemsControl control)
        {
            List<string> controlsName = new List<string>();

            foreach (TabItem item in control.Items)
            {
                controlsName.Add(item.Header.ToString());
            }

            return controlsName;
        }

        public static TableVisitInfo LoadDoctorVisits(DataTable infos,int rowLineNumber)
        {
            var info = new Dictionary<string, string>();

            foreach(DataColumn column in infos.Columns)
            {
                switch (column.ColumnName)
                {
                    case "id_reseption":
                    case "fullname_patient":
                    case "date_and_time":
                    case "user_message":
                    case "cabinet_number":
                        info.Add(column.ColumnName, infos.Rows[rowLineNumber][column.Ordinal].ToString());
                        break;
                    default:
                        break;
                }
            }


            return new TableVisitInfo
            {
                CabinetNumber = int.Parse(info["cabinet_number"]),
                dateOfVisit = DateTime.Parse(info["date_and_time"]),
                Description = info["user_message"],
                fullNamePatient = info["fullname_patient"],
                VisitID = int.Parse(info["id_reseption"])
            };
        }
    }
}
