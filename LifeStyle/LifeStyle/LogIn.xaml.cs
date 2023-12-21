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
using LifeStyle.DataBase;
using LifeStyle.ProjectFiles.Extensions;

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
            if (!ProfileHelper.Exists(this.Login.Text))
            {
                WindowHelper.ShowErrorMessageBox("Такого пользователя не существует!");
                return;
            }

            switch (ListLoginAs.SelectedItem.ToString())
            {
                case "Пациент":

                    var data1 = DBHelper.DbWorker.ExecuteFromDBCommand("SELECT * FROM patient_personal_account");

                     Switcher.SwitchWindow(this, new Client(ProfileHelper.InitializeClient(data1)));

                    break;
                case "Врач":
                    var data2 = DBHelper.DbWorker.ExecuteFromDBCommand("SELECT fullname, login, cabinet_number, specialization FROM Doctor");

                    Switcher.SwitchWindow(this, new Doctor(ProfileHelper.InitializeDoctor(data2)));

                    break;
                case "Админ":
                    var data3 = DBHelper.DbWorker.ExecuteFromDBCommand("SELECT login FROM Admin_");

                    Switcher.SwitchWindow(this, new Admin(ProfileHelper.InitializeAdmin(data3)));

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
