using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace LifeStyle.DataBase
{
    class DataBaseWorker
    {
        private readonly NpgsqlConnection connection;
        private readonly NpgsqlCommand command;
        private readonly string DataBaseName;
        private readonly string DataBaseUser;

        public DataBaseWorker(string dataBaseName,string dataBaseUser,string password,string port,string host)
        {

        }

        public void ExecuteFromDBCommand(string commands)
        {
            command.CommandText = commands;
            var reader = command.ExecuteReader();
        }

        public int ExecuteIntoDBCommand(string commands)
        {
            command.CommandText = commands;
            return command.ExecuteNonQuery();
        }
    }
}
