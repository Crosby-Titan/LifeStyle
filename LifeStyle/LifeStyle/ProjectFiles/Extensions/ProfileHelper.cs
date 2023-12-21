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

        public static Entitiy InitializeClient(DataTable entity,int rowLineNumber)
        {
            var parameters = new Dictionary<string, string>();

            foreach (DataColumn column in entity.Columns)
            {
                switch (column.ColumnName)
                {
                    case "fullname":
                    case "login":
                    case "date_of_birth":
                    case "smpnumber":
                    case "home_address":
                    case "phone_number":
                    case "passport_series":
                    case "number_passport":
                    case "passport_issued_by":
                        parameters.Add(column.ColumnName, entity.Rows[rowLineNumber][column.Ordinal].ToString());
                        break;
                    default:
                        break;
                }
            }

            return new ProjectEntities.Client(
                parameters["fullname"].Split(' '),
                DateTime.Parse(parameters["date_of_birth"]),
                parameters["login"], new Passport
                {
                    Address = parameters["home_address"],
                    From = parameters["passport_issued_by"],
                    PassportNumber = parameters["number_passport"],
                    PassportSeries = parameters["passport_series"],
                    PhoneNumber = parameters["phone_number"],
                    SMPNumber = parameters["smpnumber"]
                });
        }

        public static Entitiy InitializeDoctor(DataTable entity, int rowLineNumber)
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
                        parameters.Add(column.ColumnName, entity.Rows[rowLineNumber][column.Ordinal].ToString());
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

        public static Entitiy InitializeAdmin(DataTable entity, int rowLineNumber)
        {
            var parameters = new Dictionary<string, string>();

            foreach (DataColumn column in entity.Columns)
            {
                switch (column.ColumnName)
                {
                    case "login":
                        parameters.Add(column.ColumnName, entity.Rows[rowLineNumber][column.Ordinal].ToString());
                        break;
                    default:
                        break;
                }
            }

            return new ProjectEntities.Admin(parameters["login"]);
        }

        public static string GetClientStatus(UserStatus status)
        {
            switch(status)
            {
                case UserStatus.OnVerification:
                    return "Не проверен";
                case UserStatus.Blocked:
                    return "Заблокирован";
                case UserStatus.Deleted:
                    return "Удален";
                case UserStatus.Actual:
                    return "Актуален";
                default:
                    return string.Empty;
            }
        }

        public static void RegistrationClient(ClientRegistrationInfo registrationInfo)
        {
            DBHelper.DbWorker.ExecuteIntoDBCommand(
                $"INSERT INTO patient_personal_account " +
                $"(login,password_,fullname,date_of_birth,smpnumber,home_address,phone_number,status) " +
                $"VALUES (" +
                $"\'{registrationInfo.Email}\'," +
                $"\'{registrationInfo.PasswordHashCode}\'," +
                $"\'{String.Join(" ", registrationInfo.FullName)}\'," +
                $"\'{registrationInfo.BirthDay.ToShortDateString()}\'," +
                $"\'{registrationInfo.Passport.SMPNumber}\'," +
                $"\'{registrationInfo.Passport.Address}\'," +
                $"\'{registrationInfo.Passport.PhoneNumber}\'," +
                $"(SELECT ID FROM UserStatus WHERE status = " +
                $"\'{ProfileHelper.GetClientStatus(UserStatus.OnVerification)}\'" +
                $" LIMIT 1)" +
                $");"
                );

            DBHelper.DbWorker.ExecuteIntoDBCommand(
                $"INSERT INTO Passport (id_passport,passport_series,number_passport,passport_issued_by) " +
                $"VALUES (" +
                $"(SELECT id_patient_personal_account FROM patient_personal_account " +
                $"WHERE login = \'{registrationInfo.Email}\' LIMIT 1)," +
                $"\'{registrationInfo.Passport.PassportSeries}\'," +
                $"\'{registrationInfo.Passport.PassportNumber}\'," +
                $"\'{registrationInfo.Passport.From}\'" +
                $");"
                );
        }

        public static void RegistrationDoctor(DoctorRegistrationInfo registrationInfo)
        {
            DBHelper.DbWorker.ExecuteIntoDBCommand(
                $"INSERT INTO Doctor " +
                $"(login,password_,fullname,specialization,cabinet_number) " +
                $"VALUES " +
                $"(\'{registrationInfo.Email}\'," +
                $"\'{registrationInfo.Password}\'," +
                $"\'{String.Join(" ",registrationInfo.FullName)}\'," +
                $"\'{registrationInfo.Specialization}\'," +
                $"\'{registrationInfo.CabinetNumber}\'" +
                $");"
                );
        }
    }
}
