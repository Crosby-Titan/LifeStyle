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
using LifeStyle.WindowSwitcher;
using LifeStyle.Paths;
using LifeStyle.Extensions;

namespace LifeStyle
{
    /// <summary>
    /// Логика взаимодействия для LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        public LogIn()
        {
            InitializeComponent();
            InitializeWindowComponent();
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Switcher.SwitchWindow(this, new Registration());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            switch (ListLoginAs.SelectedItem.ToString())
            {
                case "Пациент":
                    Switcher.SwitchWindow(this, new Client());
                    break;
                case "Врач":
                    Switcher.SwitchWindow(this, new Doctor());
                    break;
                case "Админ":
                    Switcher.SwitchWindow(this, new Admin());
                    break;
                default:
                    WindowHelper.ShowErrorMessageBox("Не выбран ни один из доступных вариантов входа!");
                    break;

            }
        }

        private void InitializeLoginAs()
        {
            ListLoginAs.ItemsSource = new string[] { "Админ", "Врач", "Пациент" };
            ListLoginAs.SelectedItem = ListLoginAs.Items[2];
        }

        private void SetWindowBackground(string filename)
        {
            WindowHelper.SetWindowBackground(this, filename);
        }

        private void InitializeWindowComponent()
        {
            InitializeLoginAs();
            SetWindowBackground("login_fon.jpg");
        }
    }
}
