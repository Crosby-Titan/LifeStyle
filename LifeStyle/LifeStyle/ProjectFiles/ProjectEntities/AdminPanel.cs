using LifeStyle.DataBase;
using LifeStyle.ProjectFiles.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.ProjectFiles.ProjectEntities
{
    public class AdminPanel
    {
        private Admin _VerifiedAdmin;
        public AdminPanel(Admin admin)
        {
            if(admin == null) throw new ArgumentNullException(nameof(admin));
            if(!ProfileHelper.Exists(admin.Login)) throw new ArgumentException(nameof(admin.Login));
            _VerifiedAdmin = admin;
        }

        public void ChangeClientStatus(string email,UserStatus status)
        {
            if (string.IsNullOrEmpty(email))
                return;

            ChangeClientStatus(status, email);
        }

        private void ChangeClientStatus(UserStatus status,string email)
        {
            DBHelper.DbWorker.ExecuteIntoDBCommand(
                $"UPDATE patient_personal_account SET " +
                $"status = " +
                $"(SELECT ID FROM UserStatus WHERE status = \'{ProfileHelper.GetClientStatus(status)}\' LIMIT 1) " +
                $"WHERE login = \'{email}\';"
                );
        }

    }
}
