using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.ProjectFiles.ProjectEntities
{
    public class Entitiy: ICloneable
    {
        protected int _ID;
        protected bool _IsSuperUser;
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

        public virtual object Clone()
        {
            return new Entitiy()
            {
                _ID = this.GetHashCode(),
                _IsSuperUser = this.IsSuperUser
            };
        }

        public int ID { get { return _ID; } }
        public bool IsSuperUser { get { return _IsSuperUser; } }
    }
}
