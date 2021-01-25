using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Financial.Dac;
using School.Financial.Models;
using System.Linq;

namespace School.Financial.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SchoolController : Controller
    {
        private readonly IEducationAreaDac educationAreaDac;
        private readonly ISchoolDac schoolDac;

        public SchoolController(
            IEducationAreaDac educationAreaDac,
            ISchoolDac schoolDac
            )
        {
            this.educationAreaDac = educationAreaDac;
            this.schoolDac = schoolDac;
        }

        public IActionResult Index()
        {
            var schools = schoolDac.Get().OrderBy(x => x.sc_name);
            var educationAreas = educationAreaDac.Get();
            ViewBag.educationAreas = educationAreas;
            return View(schools);
        }

        public IActionResult Details(int id)
        {
            var school = schoolDac.Get(id);
            var educationArea = educationAreaDac.Get(school.areacode);
            ViewBag.educationArea = educationArea;
            return View(school);
        }

        public IActionResult Create()
        {
            var educationAreas = educationAreaDac.Get();
            ViewBag.educationAreas = educationAreas;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SchoolData request)
        {
            try
            {
                schoolDac.Insert(request);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Edit(int id)
        {
            var school = schoolDac.Get(id);
            var educationAreas = educationAreaDac.Get();
            ViewBag.educationAreas = educationAreas;
            return View(school);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, SchoolData request)
        {
            try
            {
                var school = schoolDac.Get(id);

                school.sc_name = request.sc_name;

                schoolDac.Update(school);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Delete(int id)
        {
            var school = schoolDac.Get(id);
            var educationArea = educationAreaDac.Get(school.areacode);
            ViewBag.educationArea = educationArea;
            return View(school);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, SchoolData request)
        {
            try
            {
                schoolDac.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
