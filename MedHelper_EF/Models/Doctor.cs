using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelper_EF.Models
{
    public partial class Doctor: BaseEntity
    {
        public Doctor()
        {
            this.Patients = new HashSet<Patient>();
        }

        public int DoctorID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }

        public virtual ICollection<Patient> Patients { get; set; }
    }
}
