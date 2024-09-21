using MaseterDEtailsCore.Models;
using MaseterDEtailsCore.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace MaseterDEtailsCore.Controllers
{
    public class CandidatesController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _env;
        public CandidatesController(AppDBContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }
        [HttpGet]
        [Route("GetSkill")]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkill()
        {
            return await _context.skills.ToListAsync();

        }
        [HttpGet]
        [Route ("GetCandidates")]
        public async Task<ActionResult<IEnumerable<Candidate>>> GetCandidates()
        {
            return await (_context.candidates.ToListAsync());
        
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandidateDTO>>> GetcandidateSkill()
        { 
             List<CandidateDTO> CanditeateSkills =new List<CandidateDTO>();
            var allCandidates= await _context.candidates.ToListAsync();
            foreach (var candidate in allCandidates)
            {
                Skill[] skillList = _context.CandidateSkills.Where(x => x.CandidateId == candidate.CandidateId).Select(x => new Skill { SkillId = x.SkillId, SkillName = x.Skill.SkillName }).ToArray();

                CanditeateSkills.Add(new CandidateDTO
                {
                    CandidateName = candidate.CandidateName,
                    CandidateId = candidate.CandidateId,
                    DateOfBirth = candidate.DateOfBirth,
                    MobileNo = candidate.MobileNo,
                    IsFresher = candidate.IsFresher,
                    Picture = candidate.Picture,
                    skillList = skillList


                });



            }
            return CanditeateSkills;



        }
        [HttpGet("{id}")]

        public async Task<ActionResult<CandidateDTO>> GetCandidateByID(int id)
        {
            Candidate candidate = await _context.candidates.FindAsync(id);
            Skill[] skillList = _context.CandidateSkills.Where(x => x.CandidateId == candidate.CandidateId).Select(x => new Skill { SkillId = x.SkillId, SkillName = x.Skill.SkillName }).ToArray();
            CandidateDTO cdto = new CandidateDTO()
            {
                CandidateName = candidate.CandidateName,
                CandidateId = candidate.CandidateId,
                DateOfBirth = candidate.DateOfBirth,
                MobileNo = candidate.MobileNo,
                IsFresher = candidate.IsFresher,
                Picture = candidate.Picture,
                skillList= skillList

            };
            return cdto;
        
        }
        [HttpPost]
        public async Task<ActionResult<CanditeateSkill>> PostCandidateSkill([FromForm] CandidateDTO dTO)
        {
            var skillItems = JsonConvert.DeserializeObject<Skill[]>(dTO.SkillStringify);
            Candidate candidate = new Candidate
            {
                CandidateName = dTO.CandidateName,
                DateOfBirth = dTO.DateOfBirth,
                MobileNo = dTO.MobileNo,
                IsFresher = dTO.IsFresher,

            };
            if (dTO.PictureFile != null)
            { 
                 var webRoot= _env.WebRootPath;
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(dTO.PictureFile.FileName);
                var filePath= Path.Combine(webRoot, "images",filename);
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                await dTO.PictureFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                fileStream.Close();
                candidate.Picture= filename;
            
            }
            foreach (var item in skillItems)
            {
                var candidateSkill = new CanditeateSkill
                {
                    Candidate = candidate,
                    CandidateId = candidate.CandidateId,
                    SkillId = item.SkillId,

                };
                _context.Add(candidateSkill);
            }
            await _context.SaveChangesAsync();
            return Ok(candidate);

        }
        [HttpPut]
        public async Task<ActionResult<CanditeateSkill>> UpdateCandidate([FromForm] CandidateDTO dTO)
        {
            var skillItems = JsonConvert.DeserializeObject<Skill[]>(dTO.SkillStringify);
            Candidate candidate = _context.candidates.Find(dTO.CandidateId);
            candidate.CandidateName= dTO.CandidateName;
            candidate.DateOfBirth= dTO.DateOfBirth;
            candidate.IsFresher = dTO.IsFresher;
            candidate.MobileNo  = dTO.MobileNo;
            if (dTO.PictureFile != null)
            { 
             var webRoot= _env.WebRootPath;
                var fileName= Guid.NewGuid().ToString()+ Path.GetExtension(dTO.PictureFile.FileName);
                var filePaht= Path.Combine(webRoot,"image", fileName);
                FileStream fileStream = new FileStream(filePaht, FileMode.Create);
                await dTO.PictureFile.CopyToAsync(fileStream);
                await fileStream .FlushAsync();
                candidate.Picture = fileName;
                
            }
            var existingSkill= _context.CandidateSkills.Where(e=>e.CandidateId== candidate.CandidateId).ToList();
            foreach (var skill in existingSkill)
            {
                _context.CandidateSkills.Remove(skill);
            }
            foreach (var skill in skillItems)
            {
                var candidateSkill = new CanditeateSkill
                {
                    CandidateId = candidate.CandidateId,
                    SkillId = skill.SkillId,

                };
                _context.Add(candidateSkill);

            }
             _context.Entry(candidate).State= EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(candidate);
                


        }
        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<ActionResult<CanditeateSkill>> DeleteCandidateSkill(int id)
        {
            Candidate candidate = _context.candidates.Find(id);
            var existingSkill = _context.CandidateSkills.Where(e => e.CandidateId == candidate.CandidateId).ToList();
            foreach (var skill in existingSkill) 
            {
                _context.CandidateSkills.Remove(skill);
            
            }
            _context.Entry(candidate).State= EntityState.Deleted;
            await _context.SaveChangesAsync();
            return Ok(candidate);
        
        }


    }
}
