using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using LifeStyle.Extensions;

namespace LifeStyle.Paths
{
#nullable enable
    public class PathWorker
    {
        public static string? ApplicationPath { get; private set; }
        public static string? Background { get; private set; }
        public static string? Icon { get; private set; }
        public static string? DataBase { get; private set; }
        public static string? Temp { get; private set; }
        public static string? XAML { get; private set; }

        static PathWorker() { InitializePaths(); }

        public static void InitializePaths()
        {
            var sb = new StringBuilder(Environment.CurrentDirectory);

            ApplicationPath = sb.Replace("bin", "_")
                .Remove(sb.IndexOf('_'))
                .ToString();

            Background = Path.Combine(ApplicationPath, "ProjectFiles\\background");
            Icon = Path.Combine(ApplicationPath, "ProjectFiles\\icon");
            DataBase = Path.Combine(ApplicationPath, "DataBase");
            Temp = Path.Combine(ApplicationPath, "ProjectFiles\\Temp");
            XAML = Path.Combine(ApplicationPath, "XAML");
        }
    }
}
