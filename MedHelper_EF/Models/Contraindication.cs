using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelper_EF.Models
{
    public partial class Contraindication
    {
        public Contraindication()
        {
            this.MedicineContraindications = new HashSet<MedicineContraindication>();
        }

        public int ContraindicationID { get; set; }
        public string Description { get; set; }
        public virtual ICollection<MedicineContraindication> MedicineContraindications { get; set; }
    }
}
