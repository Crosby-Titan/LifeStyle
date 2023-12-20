using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using LifeStyle.DataBase;
using LifeStyle.ProjectFiles.ProjectEntities;

namespace LifeStyle.ProjectFiles.Extensions
{
    public class ProfileHelper
    {
        public static bool Exists(string login)
        {
            return (IsDoctorExists(login) || IsClientExists(login) || IsAdminExists(login));
        }

        private static bool IsDoctorExists(string login)
        {
            var result = DBHelper.DbWorker.ExecuteFromDBCommand(
                $"SELECT Login FROM Doctor" +
                $" WHERE Login = \'{login}\' LIMIT 1;");

            if (result.Rows.Count < 1)
                return false;

            return true;
        }

        private static bool IsAdminExists(string login)
        {
            var result = DBHelper.DbWorker.ExecuteFromDBCommand(
                $"SELECT Login FROM Admin_" +
                $" WHERE Login = \'{login}\' LIMIT 1;");

            if (result.Rows.Count < 1)
                return false;

            return true;
        }

        private static bool IsClientExists(string login)
        {
            var result = DBHelper.DbWorker.ExecuteFromDBCommand(
                $"SELECT Login FROM Patient_personal_account" +
                $" WHERE Login = \'{login}\' LIMIT 1;");

            if (result.Rows.Count < 1)
                return false;

            return true;
        }

        public static Entitiy InitializeClient(DataTable entity)
        {
            var parameters = new Dictionary<string, string>();

            foreach(DataColumn column in entity.Columns)
            {
                switch(column.ColumnName)
                {
                    case "fullname":
                    case "login":
                    case "date_of_birth":
                        parameters.Add(column.ColumnName,entity.Rows[0][column.Ordinal].ToString());
                        break;
                    default:
                        break;
                }
            }

            return new ProjectEntities.Client(
                parameters["fullname"].Split(' '),
                DateTime.Parse(parameters["date_of_birth"]),
                parameters["login"]);
        }

        public static Entitiy InitializeDoctor(DataTable entity)
        {
            var parameters = new Dictionary<string, string>();

            foreach (DataColumn column in entity.Columns)
            {
                switch (column.ColumnName)
                {
                    case "fullname":
                    case "login":
                    case "cabinet_number":
                    case "specialization":
                        parameters.Add(column.ColumnName, entity.Rows[0][column.Ordinal].ToString());
                        break;
                    default:
                        break;
                }
            }

            return new ProjectEntities.Doctor(
                parameters["fullname"].Split(' '),
                parameters["specialization"],
                parameters["cabinet_number"],
                parameters["login"]);
        }

        public static Entitiy InitializeAdmin(DataTable entity)
        {
            var parameters = new Dictionary<string, string>();

            foreach (DataColumn column in entity.Columns)
            {
                switch (column.ColumnName)
                {
                    case "login":
                        parameters.Add(column.ColumnName, entity.Rows[0][column.Ordinal].ToString());
                        break;
                    default:
                        break;
                }
            }

            return new ProjectEntities.Admin(parameters["login"]);
        }
    }
}
