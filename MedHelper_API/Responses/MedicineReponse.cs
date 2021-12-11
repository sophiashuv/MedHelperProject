using System.Collections.Generic;

namespace MedHelper_API.Responses
{
    public class MedicineResponse
    {
        public int MedicineID { get; set; }
        public string Name { get; set; }
        public string pharmacotherapeuticGroup { get; set; }
        public List<ContraindicationResponse> Contraindications { get; set; }
        public List<CompositionResponse> Compositions { get; set; }
        public List<MedicineInteractionResponse> Interactions { get; set; }
    }
}