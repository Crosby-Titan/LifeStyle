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
        public SelectedService()
        {
            InitializeComponent();
        }

        public SelectedService(string caption) : base()
        {
            this.Title = caption;
        }

        private void SaveVisit_Click(object sender, RoutedEventArgs e)
        {
            notify.Invoke(new VisitInformation { });
        }
    }
}
