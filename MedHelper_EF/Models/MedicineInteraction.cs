using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelper_EF.Models
{
    public partial class MedicineInteraction: BaseEntity
    {
        public int MedicineInteractionID { get; set; }
        public string Description { get; set; }
        public Nullable<int> MedicineID { get; set; }
        public Nullable<int> CompositionID { get; set; }

        public virtual Composition Composition { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}
