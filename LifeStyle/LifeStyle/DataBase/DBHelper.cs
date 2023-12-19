using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LifeStyle.DataBase
{
    public static class DBHelper
    {
        public static DataBaseWorker DbWorker { get; private set; }

        static DBHelper()
        {
            var parameters = LoadDBParameters();

            InitializeDBWorker(parameters);

            ConnectWithDB(parameters.Password);
        }

        private static DBParameters LoadDBParameters()
        {
            return JsonSerializer.Deserialize<DBParameters>(File.OpenRead(""));
        }

        private static void InitializeDBWorker(DBParameters parameters)
        {
            DbWorker = new DataBaseWorker(
                    parameters.Database,
                    parameters.Username,
                    parameters.Host);
        }

        private static void ConnectWithDB(string password)
        {
            DbWorker.Connect(password);
        }
    }
}
