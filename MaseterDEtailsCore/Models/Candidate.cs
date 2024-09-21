using System.ComponentModel.DataAnnotations.Schema;

namespace MaseterDEtailsCore.Models
{
    public class Candidate
    {
        public int CandidateId { get; set; }
        public string CandidateName { get; set;}
        [Column (TypeName="Date")]
        public DateTime DateOfBirth { get; set; }
        public string? MobileNo { get; set; }
        public string? Picture { get; set; }
        public bool IsFresher { get; set; }
    }
}
