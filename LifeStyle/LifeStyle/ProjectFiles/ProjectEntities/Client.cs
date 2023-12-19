using LifeStyle.ProjectFiles.ProjectEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LifeStyle.ProjectEntities
{
    public class Client: Entitiy
    {
        private string[] _FullName;
        private string _Gender;
        private string _Email;
        private IDictionary<string, VisitInformation> _VisitHistory;

        public Client(string[] fullName, string gender,string login) : base()
        {
            _FullName = fullName;
            _Gender = gender;
            _Email = login;
            _VisitHistory = new Dictionary<string, VisitInformation>();
        }
        public string Gender { get { return _Gender; } }
        public string FullName { get { return String.Join(" ", _FullName); } }
        public string Email { get { return _Email; } }   

        public void AddVisitInHistory(string visitName, VisitInformation visitInformation)
        {
            if (!_VisitHistory.ContainsKey(visitName))
            {
                _VisitHistory.Add(visitName, visitInformation);
            }
        }
        public VisitInformation GetVisitInformation(string visitName)
        {
            if (_VisitHistory.ContainsKey(visitName))
                return _VisitHistory[visitName];

            return new VisitInformation();
        }

    }
}
