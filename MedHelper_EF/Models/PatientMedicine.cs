using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelper_EF.Models
{
    public partial class PatientMedicine
    {
        public int PatientMedicineID { get; set; }
        public Nullable<int> MedicineID { get; set; }
        public Nullable<int> PatientID { get; set; }

        public virtual Medicine Medicine { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
