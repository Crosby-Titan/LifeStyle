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

            var xaml = reader.LoadAsync(new System.IO.FileStream(@"D:\УП\LifeStyle\LifeStyle\XAML\DoctorCard.xaml", System.IO.FileMode.Open)) as ResourceDictionary;

            return xaml["DoctorCardTemplate"] as Border;
        }

        public static UIElement CreateServiceCard(Window window)
        {
            XamlReader reader = new XamlReader();

            var xaml = reader.LoadAsync(new System.IO.FileStream(@"D:\УП\LifeStyle\LifeStyle\XAML\ServiceCard.xaml", System.IO.FileMode.Open)) as ResourceDictionary;

            return xaml["ServiceCardTemplate"] as Border;
        }

        public static UIElement CreateVisitCard(Window window)
        {
            XamlReader reader = new XamlReader();

            var xaml = reader.LoadAsync(new System.IO.FileStream(@"D:\УП\LifeStyle\LifeStyle\XAML\VisitCard.xaml", System.IO.FileMode.Open)) as ResourceDictionary;

            return xaml["VisitCardTemplate"] as Border;
        }

        public static UIElement CreateVisitCard(VisitInformation visit)
        {
            XamlReader reader = new XamlReader();

            var xaml = reader.LoadAsync(new System.IO.FileStream(@"D:\УП\LifeStyle\LifeStyle\XAML\VisitCard.xaml", System.IO.FileMode.Open)) as ResourceDictionary;

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
                        btn.Content = visit.dateOfVisit.ToShortTimeString();
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
                                border.Background = new ImageBrush(new BitmapImage(visit.doctorPhoto));
                            }
                        }

                        break;
                    default:
                        break;
                }

            }

            return visitCard;
        }

        public static UIElement CreateServiceCard(ServiceInformation service)
        {
            XamlReader reader = new XamlReader();

            var xaml = reader.LoadAsync(new System.IO.FileStream(@"D:\УП\LifeStyle\LifeStyle\XAML\ServiceCard.xaml", System.IO.FileMode.Open)) as ResourceDictionary;

            var serviceCard = xaml["ServiceCardTemplate"] as Border;

            foreach(UIElement element in (serviceCard.Child as StackPanel).Children)
            {
                switch (element)
                {
                    case Label lbl:
                        lbl.Content = $"\t\t{service.ServiceName}\n\n\t{service.Description}";
                        break;
                    case Button btn:
                        btn.Click += (object sender, RoutedEventArgs e) =>
                        {
                            new SelectedService(service.ServiceName);
                        };
                        break;
                    case StackPanel panel:
                        
                        foreach(UIElement innerElement in panel.Children)
                        {
                            if(innerElement is Border doctorPhoto)
                            {
                                doctorPhoto.Background = new ImageBrush(
                                    new BitmapImage(
                                        new Uri(
                                            Path.Combine(PathWorker.Icon, "default_user_profile_image.jpg"))));
                                break;
                            }
                        }

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

            var xaml = reader.LoadAsync(new System.IO.FileStream(@"D:\УП\LifeStyle\LifeStyle\XAML\DoctorCard.xaml", System.IO.FileMode.Open)) as ResourceDictionary;

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
                                    if((lbl.Content as string).Contains("Должность", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        lbl.Content = $"\t{doctor.Specialization}\n\t{doctor.Experience}\n\tГрафик: {doctor.ShortWorkSchedule}";
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
                $"SELECT id_patient_personal_account,login,UserStatus.status FROM " +
                $"patient_personal_account,UserStatus " +
                $"WHERE UserStatus.status = \'{ProfileHelper.GetClientStatus(UserStatus.OnVerification)}\';"
                );

            var dataList = new List<object[]>();    

            foreach (DataRow request in requests.Rows)
            {
                dataList.Add(request.ItemArray);
            }

            return dataList;
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
    }
}
