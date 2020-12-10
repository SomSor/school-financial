using Microsoft.AspNetCore.Mvc;
using School.Financial.Dac;
using School.Financial.Models;
using School.Financial.Services;
using System.Linq;

namespace School.Financial.Controllers
{
    public class BankAccountController : Controller
    {
        private readonly IBankAccountDac bankAccountDac;
        private readonly IIdentityService identityService;

        public BankAccountController(
            IBankAccountDac bankAccountDac,
            IIdentityService identityService
            )
        {
            this.bankAccountDac = bankAccountDac;
            this.identityService = identityService;
        }

        public IActionResult Index()
        {
            var bankAccounts = bankAccountDac.Get()
                .OrderBy(x => x.BankName)
                .ThenBy(x => x.AccountNumber)
                .ThenBy(x => x.AccountName);
            return View(bankAccounts);
        }

        public IActionResult Details(int id)
        {
            var bankAccount = bankAccountDac.Get(id);
            return View(bankAccount);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BankAccount request)
        {
            try
            {
                var school = identityService.GetCurrentSchool();
                request.SchoolId = school.Id;
                bankAccountDac.Insert(request);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Edit(int id)
        {
            var bankAccount = bankAccountDac.Get(id);
            return View(bankAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, BankAccount request)
        {
            try
            {
                var bankAccount = bankAccountDac.Get(id);

                bankAccount.BankName = request.BankName;
                bankAccount.AccountName = request.AccountName;
                bankAccount.AccountNumber = request.AccountNumber;

                bankAccountDac.Update(bankAccount);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Delete(int id)
        {
            var bankAccount = bankAccountDac.Get(id);
            return View(bankAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, BankAccount request)
        {
            try
            {
                bankAccountDac.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
