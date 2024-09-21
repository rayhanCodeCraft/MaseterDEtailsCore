using System.ComponentModel.DataAnnotations.Schema;

namespace MaseterDEtailsCore.Models
{
    public class CanditeateSkill
    {
        public int CanditeateSkillId { get; set; }
        [ForeignKey(nameof(Candidate))]
        public int CandidateId { get; set; }    
        public int SkillId { get; set; }    
        public virtual Candidate Candidate { get; set; }
        public virtual Skill Skill { get; set; }
    }
}
