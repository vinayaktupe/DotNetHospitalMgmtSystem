using HospitalMgmtSystem.DAL.Data.Model;
using HospitalMgmtSystem.Services.Services;
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
    public class CaseFilesController : ControllerBase
    {
        private readonly ICaseFileService _caseFileService;

        public CaseFilesController(ICaseFileService caseFileService)
        {
            this._caseFileService = caseFileService;
        }
        // GET: api/<CaseFilesController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<CaseFilesController>/5
        [HttpGet("{id}")]
        [HttpGet("file/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await _caseFileService.GetCaseFileByCaseID(id);
            if (res.Count() > 0)
            {
                var response = res.Select(file => new
                {
                    file.CasePapers.DoctorID,
                    file.CasePapers.PatientID,
                    file.ID,
                    file.CaseID,
                    file.Name,
                    file.FileType,
                    file.Fields
                });

                return Ok(new
                {
                    status = "success",
                    result = response.Count(),
                    data = response.ToList()
                });
            }

            else
            {
                var file = await _caseFileService.GetCaseFileByID(id);

                if (file == null)
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
                    result = 1,
                    data = file
                });
            }

        }

        // POST api/<CaseFilesController>
        [HttpPost]
        public async Task<IActionResult> Post([Bind("CaseID,Name,FileType,Fields")] CaseFile file)
        {
            return Ok(await _caseFileService.CreateCaseFile(file));
        }

        // PUT api/<CaseFilesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<CaseFilesController>/5
        [HttpDelete("{FileID}")]
        public async Task<IActionResult> Delete(int FileID)
        {
            return Ok(await _caseFileService.DeleteCaseFile(FileID));
        }
    }
}
