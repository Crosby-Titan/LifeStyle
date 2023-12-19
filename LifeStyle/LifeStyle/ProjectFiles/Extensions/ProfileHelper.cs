using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using LifeStyle.DataBase;
using LifeStyle.ProjectEntities;
using LifeStyle.ProjectFiles.ProjectEntities;

namespace LifeStyle.ProjectFiles.Extensions
{
    public class ProfileHelper
    {
        public static bool Exists(string login)
        {
            var result = DBHelper.DbWorker.ExecuteFromDBCommand(
                $"SELECT Login FROM Patient_personal_account" +
                $"WHERE Login = {login} LIMIT 1;");

            if(result.Rows.Count != 1)
                return false;

            return true;
        }

        public static Entitiy InitializeEntity(DataTable entity)
        {
            var parameters = new List<string>();

            foreach(var column in entity.Columns)
            {
                parameters
            }

            return new LifeStyle.ProjectEntities.Client()
        }
    }
}
