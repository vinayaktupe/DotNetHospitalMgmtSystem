using HospitalMgmtSystem.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    public class HomeController : Controller
    {
        private readonly IDoctorService _doctorService;

        public HomeController(IDoctorService doctorService)
        {
            this._doctorService = doctorService;
        }
        public async Task<IActionResult> Index()
        {
            var res = await Task.Run(() => _doctorService.GetDoctorByCondition(doc => doc.Users.Email == User.Identity.Name));
            var user = res.FirstOrDefault();
            return View();
        }

        public IActionResult PastAppoinments()
        {
            return View();
        }
    }
}
