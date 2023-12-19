using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.ProjectFiles.ProjectEntities
{
    public class Entitiy
    {
        protected int _ID;
        protected bool _IsSuperUser;
        protected readonly string _Login;
        public Entitiy()
        {
            GenerateEntityID();
            _IsSuperUser = false;
        }

        protected virtual void GenerateEntityID()
        {
            _ID = this.GetHashCode();
        }

        public override string ToString()
        {
            return $"ID entity: {ID}\nIs Entity super user: {IsSuperUser}";
        }

        public int ID { get { return _ID; } }
        public bool IsSuperUser { get { return _IsSuperUser; } }
        public string Login { get { return _Login; } }
    }
}
