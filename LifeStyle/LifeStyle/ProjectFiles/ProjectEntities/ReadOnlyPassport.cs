using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.ProjectFiles.ProjectEntities
{
    public struct ReadOnlyPassport
    {
        public readonly string PassportSeries;
        public readonly string PassportNumber;
        public readonly string From;
        public readonly string Address;
        public readonly string PhoneNumber;
        public readonly string SMPNumber;
        public ReadOnlyPassport(Passport passport)
        {
            PassportSeries = passport.PassportSeries;
            PassportNumber = passport.PassportNumber;
            From = passport.From;
            Address = passport.Address;
            PhoneNumber = passport.PhoneNumber;
            SMPNumber = passport.SMPNumber;
        }

    }
}
