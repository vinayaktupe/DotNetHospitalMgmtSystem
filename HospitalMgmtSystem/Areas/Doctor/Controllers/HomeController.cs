using HospitalMgmtSystem.DAL.Data.Model;
using HospitalMgmtSystem.Services.Services;
using HospitalMgmtSystem.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    public class HomeController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly ICasePaperService _casePaperService;

        public HomeController(IDoctorService doctorService, ICasePaperService casePaperService)
        {
            this._doctorService = doctorService;
            this._casePaperService = casePaperService;
        }
        public async Task<IActionResult> Index()
        {
            var cases = await _casePaperService
                .GetCasePaperByCondition(paper => 
                paper.Doctors.Users.Email == User.Identity.Name && 
                paper.IsSolved == false && 
                paper.IsActive == true);


            return View(cases.Select(paper => new CasePaperViewModel
            {
                ID = paper.ID,
                DoctorName = paper.Doctors.Users.FirstName + " " + paper.Doctors.Users.LastName,
                PatientName = paper.PatientName,
                SelfName = paper.Patients.Users.FirstName + " " + paper.Patients.Users.LastName,
                Description = paper.Description,
                IsSolved = paper.IsSolved,
                ForSelf = paper.ForSelf,
                MedicalHistory = paper.Patients.MedicalHistory
            }));
        }

        public async Task<IActionResult> PastAppoinments()
        {
            var cases = await _casePaperService
                .GetCasePaperByCondition(paper => 
                paper.Doctors.Users.Email == User.Identity.Name && 
                paper.IsSolved == true && 
                paper.IsActive == true);


            return View(cases.Select(paper => new CasePaperViewModel
            {
                ID = paper.ID,
                DoctorName = paper.Doctors.Users.FirstName + " " + paper.Doctors.Users.LastName,
                PatientName = paper.PatientName,
                SelfName = paper.Patients.Users.FirstName + " " + paper.Patients.Users.LastName,
                Description = paper.Description,
                IsSolved = paper.IsSolved,
                ForSelf = paper.ForSelf,
                MedicalHistory = paper.Patients.MedicalHistory
            }));
        }

        public async Task<IActionResult> Details(int id)
        {
            var res = await _casePaperService
                .GetCasePaperByCondition(paper => 
                paper.Doctors.Users.Email == User.Identity.Name && 
                paper.ID == id && 
                paper.IsActive == true);
            
            if (res == null)
            {
                return NotFound();
            }

            var casePaper = res.SingleOrDefault();

            return View(new CasePaperViewModel()
            {
                ID = casePaper.ID,
                DoctorName = casePaper.Doctors.Users.FirstName + " " + casePaper.Doctors.Users.LastName,
                SelfName = casePaper.Patients.Users.FirstName + " " + casePaper.Patients.Users.LastName,
                ForSelf = casePaper.ForSelf,
                MedicalHistory = casePaper.Patients.MedicalHistory,
                PatientName = casePaper.PatientName,
                Description = casePaper.Description,
                IsSolved = casePaper.IsSolved,
                Specialization = casePaper.Doctors.Specialization
            });
        }
    }
}
