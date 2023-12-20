using LifeStyle.ProjectFiles.ProjectEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.ProjectFiles.Events
{
    public interface IRegistrationEvent
    {
        public delegate void NotifyParent(ClientRegistrationInfo registrationInfo);
        public event NotifyParent notify;
    }
}
