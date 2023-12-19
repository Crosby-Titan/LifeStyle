using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.ProjectFiles.ProjectEntities
{
    public struct VisitInformation
    {
        public string fullNameDoctor;
        public string fullNamePatient;
        public DateTime dateOfVisit;
        public string ServiceName;
        public int ServiceID;
        public string Description;
        public Uri doctorPhoto;
    }
}
