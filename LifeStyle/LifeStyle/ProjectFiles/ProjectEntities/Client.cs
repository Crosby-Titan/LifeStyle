using LifeStyle.ProjectFiles.ProjectEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LifeStyle.ProjectFiles.ProjectEntities
{
    public class Client: Entitiy, ICloneable
    {
        private string[] _FullName;
        private string _Email;
        private DateTime _BirthDay;
        private IDictionary<string, VisitInformation> _VisitHistory;
        private Passport _Passport;
        private UserStatus _Status;

        public Client(string[] fullName, DateTime birthday,string login, Passport passport) : base()
        {
            _FullName = fullName;
            _Email = login;
            _BirthDay = birthday;
            _VisitHistory = new Dictionary<string, VisitInformation>();
            _Passport = passport;
        }
        public string FullName { get { return String.Join(" ", _FullName); } }
        public string Email { get { return _Email; } }   
        public ReadOnlyPassport Passport { get { return new ReadOnlyPassport(_Passport); } }
        public UserStatus Status { get { return _Status; } set { _Status = value; } }
        public DateTime BirthDay {  get { return _BirthDay; } }

        public void AddVisitInHistory(string visitName, VisitInformation visitInformation)
        {
            if (!_VisitHistory.ContainsKey(visitName))
            {
                _VisitHistory.Add(visitName, visitInformation);
            }
        }

        public override object Clone()
        {
            return new Client(_FullName.Clone() as string[], _BirthDay, _Email.Clone() as string,_Passport)
            {
                _VisitHistory = new Dictionary <string, VisitInformation>(_VisitHistory),
                _Status = _Status,
                _IsSuperUser = _IsSuperUser,
                _Passport = _Passport
            };

        }

        public VisitInformation GetVisitInformation(string visitName)
        {
            if (_VisitHistory.ContainsKey(visitName))
                return _VisitHistory[visitName];

            return new VisitInformation();
        }

    }
}
