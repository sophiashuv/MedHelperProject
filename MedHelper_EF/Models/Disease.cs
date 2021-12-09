using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelper_EF.Models
{
    public partial class Disease: BaseEntity
    {
        public Disease()
        {
            this.PatientDiseases = new HashSet<PatientDisease>();
        }

        public int DiseaseID { get; set; }
        public string Title { get; set; }

        public virtual ICollection<PatientDisease> PatientDiseases { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
