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


namespace LifeStyle
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        private IDictionary<string,Page> _RegistrationPages;
        public Registration()
        {
            InitializeComponent();
            InitializeWindowComponent();
        }

        private void InitializeWindowComponent()
        {
            SetWindowBackground("login_fon.jpg");
            InitializePageDictionary();
            InitializePages();
            InitializeFrameHost();
        }

        public void InitializePageDictionary()
        {
            _RegistrationPages = new Dictionary<string, Page>();
        }
        private void InitializeFrameHost()
        {
            RegistrationPageHost.Content = _RegistrationPages[nameof(FirstRegistration)];
        }

        private void InitializePages()
        {
            var firstPage = new FirstRegistration();

            firstPage.notify += SetNextPage;

            var secondPage = new PersonalDataInput();

            secondPage.notify += RegistrationEnded;

            _RegistrationPages.Add(nameof(FirstRegistration), firstPage);
            _RegistrationPages.Add(nameof(PersonalDataInput), secondPage);
        }
        private void SetNextPage()
        {
            RegistrationPageHost.Navigate(_RegistrationPages[nameof(PersonalDataInput)]);
        }

        private void RegistrationEnded()
        {
            Switcher.SwitchWindow(this, new Client());
        }

        private void SetWindowBackground(string filename)
        {
            WindowHelper.SetWindowBackground(this, filename);
        }


    }
}
