using Microsoft.AspNetCore.Mvc;
using School.Financial.Dac;
using School.Financial.Models;
using School.Financial.Services;
using System.Linq;

namespace School.Financial.Controllers
{
    public class BudgetController : Controller
    {
        private readonly IBankAccountDac bankAccountDac;
        private readonly IBudgetDac budgetDac;
        private readonly IIdentityService identityService;

        private SchoolData _currentSchoolData { get; set; }
        public SchoolData CurrentSchoolData { get { return _currentSchoolData ??= identityService.GetCurrentSchool(); } }

        public BudgetController(
            IBankAccountDac bankAccountDac,
            IBudgetDac budgetDac,
            IIdentityService identityService
            )
        {
            this.bankAccountDac = bankAccountDac;
            this.budgetDac = budgetDac;
            this.identityService = identityService;
        }

        public IActionResult Index()
        {
            var budgets = budgetDac.Get().Where(x => x.SchoolId == CurrentSchoolData.sc_id).OrderBy(x => x.Name);
            var bankAccounts = bankAccountDac.Get();
            ViewBag.bankAccounts = bankAccounts;
            return View(budgets);
        }

        public IActionResult Details(int id)
        {
            var budget = budgetDac.Get(id);
            var bankAccount = bankAccountDac.Get(budget.BankAccountId);
            ViewBag.bankAccount = bankAccount;
            return View(budget);
        }

        public IActionResult Create()
        {
            var bankAccounts = bankAccountDac.Get();
            ViewBag.bankAccounts = bankAccounts;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Budget request)
        {
            try
            {
                var school = identityService.GetCurrentSchool();
                request.SchoolId = school.sc_id;
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
            var bankAccount = bankAccountDac.Get(budget.BankAccountId);
            ViewBag.bankAccount = bankAccount;
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
            var bankAccount = bankAccountDac.Get(budget.BankAccountId);
            ViewBag.bankAccount = bankAccount;
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
