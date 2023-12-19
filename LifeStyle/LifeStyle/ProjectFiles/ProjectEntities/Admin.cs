using LifeStyle.ProjectFiles.ProjectEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.ProjectFiles.ProjectEntities
{
    public class Admin: Entitiy, ICloneable
    {
        private string _Login;

        public Admin(string login)
        {
            _Login = login;
            _IsSuperUser = true;
        }

        public string Login { get { return _Login; } }

        public override object Clone()
        {
            return new Admin(_Login.Clone() as string);
        }
    }
}
