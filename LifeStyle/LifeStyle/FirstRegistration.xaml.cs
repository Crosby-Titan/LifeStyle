using LifeStyle.ProjectFiles.Events;
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
    /// Логика взаимодействия для FirstRegistration.xaml
    /// </summary>
    public partial class FirstRegistration : Page, IRegistrationEvent
    {
        public FirstRegistration()
        {
            InitializeComponent();
        }

        public event IRegistrationEvent.NotifyParent notify;

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            notify.Invoke(new ProjectFiles.ProjectEntities.ClientRegistrationInfo
            {
                Email = user_email_address.Text,
                BirthDay = new DateTime(user_date_of_birth.SelectedDate.Value.Ticks),
                FullName = new[]{user_name.Text,user_second_name.Text,user_third_name.Text },
                PasswordHashCode = user_password.GetHashCode(),
            });
        }
    }
}
