using HospitalMgmtSystem.DAL.Data.Model;
using HospitalMgmtSystem.Services.Services;
using HospitalMgmtSystem.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HospitalMgmtSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CasePapersController : ControllerBase
    {
        private readonly ICasePaperService _casePaperService;

        public CasePapersController(ICasePaperService casePaperService)
        {
            this._casePaperService = casePaperService;
        }

        // GET: api/<CasePapersController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var papers = await _casePaperService.GetAllCasePapers();

            if (papers == null)
            {
                return BadRequest(new
                {
                    status = "fail",
                    results = 0
                });
            }

            return Ok(new
            {
                status = "success",
                results = papers.Count(),
                data = papers.ToList()
            });
        }

        // GET api/<CasePapersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var paper = await _casePaperService.GetCasePaperByID(id);

            if (paper == null)
            {
                return BadRequest(new
                {
                    status = "fail",
                    results = 0
                });
            }

            return Ok(new
            {
                status = "success",
                data = new
                {
                    paper.ID,
                    DoctorID = paper.DoctorID,
                    DoctorName = paper.Doctors.Users.FirstName + " " + paper.Doctors.Users.LastName,
                    PatientID = paper.PatientID,
                    PatientName = paper.Patients.Users.FirstName + " " + paper.Patients.Users.LastName,
                    paper.Description,
                    paper.Patients.MedicalHistory
                }
            });

        }

        // POST api/<CasePapersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CasePaperRegisterModel receivedCasePaper)
        {
            CasePaper casePaper = new CasePaper()
            {
                DoctorID = receivedCasePaper.DoctorID,
                PatientID = receivedCasePaper.PatientID,
                Description = receivedCasePaper.Description,
                ForSelf = receivedCasePaper.ForSelf,
                PatientName = receivedCasePaper.PatientName
            };

            var res = _casePaperService.CreateCasePaper(casePaper);

            if (res == null)
            {
                return BadRequest();
            }
            return NoContent();
        }

        // PUT api/<CasePapersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [Bind("ID,Specialization,DoctorID,PatientID,PatientName,Description,ForSelf")] CasePaperRegisterModel receivedCasePaper)
        {
            CasePaper casePaper = await _casePaperService.GetCasePaperByID(id);
            if (casePaper == null)
            {
                return BadRequest();
            }

            casePaper.DoctorID = receivedCasePaper.DoctorID == casePaper.DoctorID || receivedCasePaper.DoctorID == 0
                                ? casePaper.DoctorID
                                : receivedCasePaper.DoctorID;

            casePaper.PatientID = receivedCasePaper.PatientID == casePaper.PatientID || receivedCasePaper.PatientID == 0
                                ? casePaper.PatientID
                                : receivedCasePaper.PatientID;

            casePaper.Description = receivedCasePaper.Description ?? casePaper.Description;
            casePaper.ForSelf = receivedCasePaper.ForSelf == casePaper.ForSelf || receivedCasePaper.PatientName == null 
                                ? casePaper.ForSelf 
                                : receivedCasePaper.ForSelf;

            casePaper.PatientName = receivedCasePaper.PatientName ?? casePaper.PatientName;

            await _casePaperService.UpdateCasePaper(casePaper);

            return Ok(new
            {
                casePaper.ID,
                casePaper.PatientID,
                casePaper.PatientName,
                casePaper.Description,
                casePaper.IsSolved,
                casePaper.ForSelf
            });

        }

        // DELETE api/<CasePapersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _casePaperService.DeleteCasePaper(id));
        }
    }
}
