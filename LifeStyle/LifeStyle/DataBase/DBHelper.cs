using LifeStyle.Paths;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using LifeStyle.Extensions;

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
            using var stream = File.OpenRead(Path.Combine(PathWorker.DataBase,"data_base_parameters.json"));

            var bytes = new Span<byte>(new byte[stream.Length]);

            stream.Read(bytes);

            string json = Encoding.UTF8.GetString(bytes);

            return JsonSerializer.Deserialize<DBParameters>(json);
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
            if (!DbWorker.Connect(password))
            {
                WindowHelper.ShowErrorMessageBox($"DataBase connection is {DbWorker.ConnectionStatus}");
                Environment.Exit(-1);
            }
            
        }

    }
}
