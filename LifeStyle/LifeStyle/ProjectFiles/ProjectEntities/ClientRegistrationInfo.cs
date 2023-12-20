using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.ProjectFiles.ProjectEntities
{
    public struct ClientRegistrationInfo
    {
        public string[] FullName;
        public string Email;
        public DateTime BirthDay;
        public int PasswordHashCode;
        public Passport Passport;
    }
}
