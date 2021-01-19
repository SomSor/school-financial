using Microsoft.AspNetCore.Mvc;
using School.Financial.Dac;
using School.Financial.Models;
using School.Financial.Services;
using System.Linq;

namespace School.Financial.Controllers
{
    public class SchoolYearController : Controller
    {
        private readonly ISchoolYearDac schoolYearDac;
        private readonly IIdentityService identityService;

        private SchoolData _currentSchoolData { get; set; }
        public SchoolData CurrentSchoolData { get { return _currentSchoolData ??= identityService.GetCurrentSchool(); } }

        public SchoolYearController(
            ISchoolYearDac schoolYearDac,
            IIdentityService identityService
            )
        {
            this.schoolYearDac = schoolYearDac;
            this.identityService = identityService;
        }

        public IActionResult Index()
        {
            var schoolYears = schoolYearDac.Get()
                .Where(x => x.SchoolId == CurrentSchoolData.sc_id)
                .OrderBy(x => x.Year);
            return View(schoolYears);
        }

        public IActionResult Details(int id)
        {
            var schoolYear = schoolYearDac.Get(id);
            return View(schoolYear);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SchoolYear request)
        {
            try
            {
                var school = identityService.GetCurrentSchool();
                request.SchoolId = school.sc_id;
                schoolYearDac.Insert(request);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Edit(int id)
        {
            var schoolYear = schoolYearDac.Get(id);
            return View(schoolYear);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, SchoolYear request)
        {
            try
            {
                var schoolYear = schoolYearDac.Get(id);

                schoolYear.Year = request.Year;
                schoolYear.StartDate = request.StartDate;

                schoolYearDac.Update(schoolYear);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Delete(int id)
        {
            var schoolYear = schoolYearDac.Get(id);
            return View(schoolYear);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, SchoolYear request)
        {
            try
            {
                schoolYearDac.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
