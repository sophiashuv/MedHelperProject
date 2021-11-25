using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelper_EF.Models
{
    public partial class Patient: BaseEntity
    {
        public Patient()
        {
            PatientDiseases = new HashSet<PatientDisease>();
            PatientMedicines = new HashSet<PatientMedicine>();
        }

        public int PatientID { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public int DoctorID { get; set; }
        public List<int> MedicineIds { get; set; }
        public System.DateTime Birthdate { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual ICollection<PatientDisease> PatientDiseases { get; set; }
        public virtual ICollection<PatientMedicine> PatientMedicines { get; set; }
    }
}
