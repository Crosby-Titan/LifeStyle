using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LifeStyle.WindowSwitcher
{
    static class Switcher
    {
        public static void SwitchWindow(Window toClose,Window toOpen)
        {
            toClose.Close();

            toOpen.Show();
        }
    }
}
