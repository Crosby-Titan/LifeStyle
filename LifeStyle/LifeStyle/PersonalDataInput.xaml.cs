using LifeStyle.Extensions;
using LifeStyle.ProjectFiles.Events;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LifeStyle
{
    /// <summary>
    /// Логика взаимодействия для PersonalDataInput.xaml
    /// </summary>
    public partial class PersonalDataInput : Page, IRegistrationEvent
    {
        public PersonalDataInput()
        {
            InitializeComponent();
        }

        public event IRegistrationEvent.NotifyParent notify;

        private void FinishRegistration_Click(object sender, RoutedEventArgs e)
        {
            notify.Invoke(new ProjectFiles.ProjectEntities.ClientRegistrationInfo
            {
                 Passport = new ProjectFiles.ProjectEntities.Passport
                 {
                      Address = registration_address.Text,
                      From = registration_passport_from.Text,
                      PassportNumber = registration_passport_number.Text,
                      PassportSeries = registration_passport_series.Text,
                      PhoneNumber = user_phone_number.Text,
                      SMPNumber = registration_smp_number.Text,
                 }
            });
        }
    }
}

