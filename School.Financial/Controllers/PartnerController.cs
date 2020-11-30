using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Financial.Dac;
using School.Financial.Models;
using System.Linq;

namespace School.Financial.Controllers
{
    public class PartnerController : Controller
    {
        private readonly IPartnerDac partnerDac;

        public PartnerController(IPartnerDac partnerDac)
        {
            this.partnerDac = partnerDac;
        }

        public ActionResult Index()
        {
            var partners = partnerDac.Get().OrderBy(x => x.Name);
            return View(partners);
        }

        public ActionResult Details(int id)
        {
            var partner = partnerDac.Get(id);
            return View(partner);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Partner request)
        {
                partnerDac.Insert(request);
                return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(int id)
        {
            var partner = partnerDac.Get(id);
            return View(partner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Partner request)
        {
            try
            {
                var partner = partnerDac.Get(id);

                partner.Name = request.Name;
                partner.VatNumber = request.VatNumber;
                partner.Address = request.Address;
                partner.PartnerType = request.PartnerType;

                partnerDac.Update(partner);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var partner = partnerDac.Get(id);
            return View(partner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                partnerDac.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
