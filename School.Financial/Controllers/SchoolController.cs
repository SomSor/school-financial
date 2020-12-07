using Microsoft.AspNetCore.Mvc;
using School.Financial.Dac;
using School.Financial.Models;
using System.Linq;

namespace School.Financial.Controllers
{
    public class SchoolController : Controller
    {
        private readonly ISchoolDac schoolDac;

        public SchoolController(ISchoolDac schoolDac)
        {
            this.schoolDac = schoolDac;
        }

        public IActionResult Index()
        {
            var schools = schoolDac.Get().OrderBy(x => x.Name);
            return View(schools);
        }

        public IActionResult Details(int id)
        {
            var school = schoolDac.Get(id);
            return View(school);
        }

        public IActionResult Create()
        {
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
            return View(school);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, SchoolData request)
        {
            try
            {
                var school = schoolDac.Get(id);

                school.Name = request.Name;

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
