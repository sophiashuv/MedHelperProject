using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelper_EF.Models
{
    public partial class MedicineContraindication
    {
        public int MedicineContraindicationID { get; set; }
        public Nullable<int> MedicineID { get; set; }
        public Nullable<int> ContraindicationID { get; set; }

        public virtual Contraindication Contraindication { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}
