using Microsoft.AspNetCore.Mvc;
using School.Financial.Dac;
using School.Financial.Models;
using System.Linq;

namespace School.Financial.Controllers
{
    public class BudgetController : Controller
    {
        private readonly IBudgetDac budgetDac;

        public BudgetController(IBudgetDac budgetDac)
        {
            this.budgetDac = budgetDac;
        }

        public IActionResult Index()
        {
            var budgets = budgetDac.Get().OrderBy(x => x.Name);
            return View(budgets);
        }

        public IActionResult Details(int id)
        {
            var budget = budgetDac.Get(id);
            return View(budget);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Budget request)
        {
            try
            {
                budgetDac.Insert(request);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Edit(int id)
        {
            var budget = budgetDac.Get(id);
            return View(budget);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Budget request)
        {
            try
            {
                var budget = budgetDac.Get(id);

                budget.Name = request.Name;

                budgetDac.Update(budget);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Delete(int id)
        {
            var budget = budgetDac.Get(id);
            return View(budget);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Budget request)
        {
            try
            {
                budgetDac.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
