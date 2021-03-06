﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Financial.Dac;
using School.Financial.Models;
using System.Linq;

namespace School.Financial.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EducationAreaController : Controller
    {
        private readonly IEducationAreaDac educationAreaDac;

        public EducationAreaController(IEducationAreaDac educationAreaDac)
        {
            this.educationAreaDac = educationAreaDac;
        }

        public IActionResult Index()
        {
            var educationAreas = educationAreaDac.Get().OrderBy(x => x.sao_name);
            return View(educationAreas);
        }

        public IActionResult Details(int id)
        {
            var educationArea = educationAreaDac.Get(id);
            return View(educationArea);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EducationArea request)
        {
            try
            {
                educationAreaDac.Insert(request);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Edit(int id)
        {
            var educationArea = educationAreaDac.Get(id);
            return View(educationArea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EducationArea request)
        {
            try
            {
                var educationArea = educationAreaDac.Get(id);

                educationArea.sao_name = request.sao_name;

                educationAreaDac.Update(educationArea);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Delete(int id)
        {
            var educationArea = educationAreaDac.Get(id);
            return View(educationArea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, EducationArea request)
        {
            try
            {
                educationAreaDac.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
