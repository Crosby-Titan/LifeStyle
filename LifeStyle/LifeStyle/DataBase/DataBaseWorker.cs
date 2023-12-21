using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace LifeStyle.DataBase
{
    public class DataBaseWorker
    {
        private  NpgsqlConnection _connection;
        private readonly NpgsqlCommand _command;
        private readonly string _DataBaseName;
        private readonly string _DataBaseUser;
        private readonly int _Port;
        private readonly string _Host;
        private ConnectionStatus _ConnectionStatus;

        public DataBaseWorker(string dataBaseName,string dataBaseUser,string host, int port = 0)
        {
            _DataBaseName = dataBaseName;
            _DataBaseUser = dataBaseUser;
            _Host = host;
            _Port = port;
            _command = new NpgsqlCommand();
        }

        public string LastQueryText { get { return _command.CommandText; } }
        public ConnectionStatus ConnectionStatus { get { return _ConnectionStatus; } }

        public DataTable ExecuteFromDBCommand(string commands)
        {
            _command.CommandText = commands;

            var reader = _command.ExecuteReader();
            var table = new DataTable();

            foreach(var column in reader.GetColumnSchema())
            {
                table.Columns.Add(column.ColumnName, column.DataType);
            }

            while (reader.Read())
            {
                var parameters = new List<object>(reader.FieldCount);
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    parameters.Add(reader.GetValue(i));
                }
                table.Rows.Add(parameters.ToArray());
            }

            reader.Close();

            return table;
        }

        public int ExecuteIntoDBCommand(string commands)
        {
            _command.CommandText = commands;
            return _command.ExecuteNonQuery();
        }

        public bool Connect(string password)
        {
            try
            {
                _connection = new NpgsqlConnection(
                    $"Host={_Host};" +
                    $"Username={_DataBaseUser};" +
                    $"Password={password};" +
                    $"Database={_DataBaseName}"
                    );

                _connection.Open();

                _command.Connection = _connection;

                _ConnectionStatus = ConnectionStatus.Connected;

                return true;
            }
            catch(NpgsqlException ex)
            {
                _ConnectionStatus = ConnectionStatus.Broken;
                Extensions.WindowHelper.ShowErrorMessageBox($"Connection state is {_connection.State}\nError message: {ex.Message}");
                return false;
            }
            catch(ArgumentException ex)
            {
                _ConnectionStatus = ConnectionStatus.Broken;
                Extensions.WindowHelper.ShowErrorMessageBox($"Connection state is {_connection.State}\nError message: {ex.Message}");
                return false;
            }
        }
    }
}
