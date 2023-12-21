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

        public void ChangeClientStatus(Entitiy client,UserStatus status)
        {
            if (client is null)
                return;

            (client as Client).Status = status;

            ChangeClientStatus(client);
        }

        private void ChangeClientStatus(Entitiy client)
        {
            DBHelper.DbWorker.ExecuteIntoDBCommand(
                $"UPDATE patient_personal_account SET" +
                $"status = {ProfileHelper.GetClientStatus((client as Client).Status)} " +
                $"WHERE login = {(client as Client).Email};"
                );
        }

    }
}
