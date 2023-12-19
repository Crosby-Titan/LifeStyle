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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LifeStyle
{
    /// <summary>
    /// Логика взаимодействия для PersonalDataInput.xaml
    /// </summary>
    public partial class PersonalDataInput : Page
    {
        public delegate void NotifyParent();
        public event NotifyParent notify;
        public PersonalDataInput()
        {
            InitializeComponent();
        }

        private void FinishRegistration_Click(object sender, RoutedEventArgs e)
        {
            notify.Invoke();
        }
    }
}

