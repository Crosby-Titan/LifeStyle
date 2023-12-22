using LifeStyle.ProjectFiles.ProjectEntities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.ProjectFiles.ProjectEntities
{
    public class Doctor: Entitiy, ICloneable
    {
        private string[] _FullName;
        private string _Email;
        private string _Specialization;
        private string _CabinetNumber;
        private Uri _DoctorsPhoto;
        private int _Experience;
        private IDictionary<ScheduleDay,Schedule> _WorkSchedule;
        private ICollection<Client> _PinnedPatients;
        public Doctor(string[] fullName,string specialization,string cabinetNumber,string login)
        {
            _FullName = fullName;
            _Specialization = specialization;
            _CabinetNumber = cabinetNumber;
            _Email = login;
            _PinnedPatients = new LinkedList<Client>();
            _WorkSchedule = new Dictionary<ScheduleDay, Schedule>();
        }

        public string FullName { get { return String.Join(" ", _FullName); } }
        public string Email { get { return _Email; } }
        public string Specialization { get { return _Specialization; } }
        public string CabinetNumber { get {  return _CabinetNumber; } }
        public Uri DoctorsPhoto { get {  return _DoctorsPhoto; } }
        public int Experience { get { return _Experience; } }
        public ReadOnlyCollection<Client> PinnedPatients { get { return new ReadOnlyCollection<Client>(PinnedPatients); } }
        public ReadOnlyDictionary<ScheduleDay, Schedule> WorkSchedule { get { return new ReadOnlyDictionary<ScheduleDay, Schedule>(_WorkSchedule); } }
        public string ShortWorkSchedule
        {
            get
            {
                const int Week = 7;
                int workingDays = _WorkSchedule.Count((keyValue) =>
                {
                    if (keyValue.Value.IsWorkingToday)
                        return true;

                    return false;
                });

                return $"{workingDays}/{Week - workingDays}";
                
            }
        }
        
        public void SetPhoto(Uri photoUri)
        {
            _DoctorsPhoto = new Uri(photoUri.OriginalString);
        }

        public void AddExperience(int experience)
        {
            _CabinetNumber += experience;
        }

        public void SetEmail(string email)
        {
            _Email = email;
        }

        public bool PinPatient(Client client)
        {
            if (!_PinnedPatients.Contains(client))
            {
                _PinnedPatients.Add(client);
                return true;
            }

            return false;
        }

        public Client? GetPinnedClient(int id)
        {
            foreach(Client client in _PinnedPatients)
            {
                if (client.ID == id) return client;
            }
            
            return null;
        }

        public override object Clone()
        {
            return new Doctor(_FullName, _Specialization, _CabinetNumber,_Email)
            {
                _IsSuperUser = _IsSuperUser,
                //_DoctorsPhoto = new Uri(_DoctorsPhoto.OriginalString),
                _ID = _ID,
                _Experience = _Experience,
                _PinnedPatients = new LinkedList<Client>(_PinnedPatients),
                _WorkSchedule = new Dictionary<ScheduleDay, Schedule>(_WorkSchedule)
            };
        }
    }
}
