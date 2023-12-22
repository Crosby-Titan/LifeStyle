using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.ProjectFiles.ProjectEntities
{
    public struct TableVisitInfo
    {
        public string fullNamePatient { get;set; }
        public DateTime dateOfVisit { get; set; }
        public int VisitID { get; set; }
        public string Description { get; set; }
        public int CabinetNumber { get; set; }
    }
}
