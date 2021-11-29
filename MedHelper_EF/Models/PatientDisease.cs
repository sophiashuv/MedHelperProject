using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelper_EF.Models
{
    public partial class PatientDisease
    {
        public int PatientDiseaseID { get; set; }
        public int PatientID { get; set; }
        public int DiseaseID { get; set; }

        public virtual Disease Disease { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
