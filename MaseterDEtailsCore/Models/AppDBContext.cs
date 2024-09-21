using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MaseterDEtailsCore.Models
{
    public class AppDBContext :DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext>options):base(options) 
        {

            
        }
        public DbSet<Skill> skills { get; set; }
        public DbSet <Candidate> candidates { get; set; }
        public DbSet<CanditeateSkill> CandidateSkills { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(

                new Skill { SkillId= 1,SkillName="C#"},
                new Skill { SkillId= 2,SkillName="SQL"},
                new Skill { SkillId= 3,SkillName="JAVA"},
                new Skill { SkillId= 4,SkillName="ASP.NET"}
               
            
            );

        }
    }
}
