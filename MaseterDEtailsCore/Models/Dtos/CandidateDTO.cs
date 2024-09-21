using System.ComponentModel.DataAnnotations.Schema;

namespace MaseterDEtailsCore.Models.Dtos
{
    public class CandidateDTO
    {
        public int CandidateId { get; set; }
        public string CandidateName { get; set; }
        [Column(TypeName = "Date")]
        public DateTime DateOfBirth { get; set; }
        public string? MobileNo { get; set; }
        public string? Picture { get; set; }
        public bool IsFresher { get; set; }

        public IFormFile PictureFile { get; set; }
        public Skill[]? skillList { get; set; }  
        public string SkillStringify { get; set; } 
    }
}
