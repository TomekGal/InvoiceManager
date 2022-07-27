using InvoiceManager.Models.Domains;
using InvoiceManager.Models.Repositories;
using InvoiceManager.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace InvoiceManager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private InvoiceRepository _invoiceRepository = new InvoiceRepository();
        private ClientRepository _clientRepository = new ClientRepository();
        private ProductRepository _productRepository = new ProductRepository();
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var invoices = _invoiceRepository.GetInvoices(userId);

            return View(invoices);
        }

        public ActionResult Invoice(int id = 0)
        {
            var userId = User.Identity.GetUserId();

            var invoice = id == 0 ? GetNewInvoice(userId) : _invoiceRepository.GetInvoice(id, userId);

            var vm = PrepareInvoiceViewModel(invoice, userId);

            return View(vm);
        }

        private EditInvoiceViewModel PrepareInvoiceViewModel(Invoice invoice, string userId)
        {
            return new EditInvoiceViewModel
            {
                Invoice = invoice,
                Heading = invoice.Id == 0 ? "Dodawanie nowej faktury" : "Faktura",
                Clients = _clientRepository.GetClients(userId),
                MethodOfPayments = _invoiceRepository.GetMethodsOfPayment()
            };
        }

        private Invoice GetNewInvoice(string userId)
        {
            var title = GetTitle();
            return new Invoice
            {
                Title = title,
                UserId = userId,
                CreatedDate = DateTime.Now,
                PaymentDate = DateTime.Now.AddDays(7)
            };
        }

        private string GetTitle()
        {
            var lastInvoiceNumber = _invoiceRepository.GetLastInvoiceNumber() + 1;
            var title = $"FA/{lastInvoiceNumber}/{DateTime.Today.Month.ToString()}/{DateTime.Today.Year.ToString()}";
            return title;
        }

        public ActionResult InvoicePosition(int invoiceId, int invoicePositionId = 0)
        {

            var userId = User.Identity.GetUserId();
            var invoicePositionLp = _invoiceRepository.GetLastPositionOfInvoice(invoiceId);

            var invoicePosition = invoicePositionId == 0 ? GetNewPosition(invoiceId, invoicePositionId, invoicePositionLp) :
              _invoiceRepository.GetInvoicePosition(invoicePositionId, userId);


            var vm = PrepareInvoicePositionVm(invoicePosition);

            return View(vm);
        }

        public ActionResult InvoicePositionRefresh(int invoiceId)
        {

            var userId = User.Identity.GetUserId();


            _invoiceRepository.RefreshPositions(invoiceId);
            var invoice = _invoiceRepository.GetInvoice(invoiceId, userId);

            var vm = PrepareInvoiceViewModel(invoice, userId);

            return RedirectToAction("Invoice", new { id = invoiceId });

        }

        private EditInvoicePositionViewModel PrepareInvoicePositionVm(InvoicePosition invoicePosition)
        {
            return new EditInvoicePositionViewModel
            {
                InvoicePosition = invoicePosition,
                Heading = invoicePosition.Id == 0 ? "Dodawanie nowej pozycji" :
                "Edycja pozycji",
                Products = _productRepository.GetProducts(),


            };
        }

        private InvoicePosition GetNewPosition(int invoiceId, int invoicePositionId, int invoiceLp)
        {

            return new InvoicePosition
            {
                InvoiceId = invoiceId,
                Id = invoicePositionId,
                Lp = invoiceLp + 1
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Invoice(Invoice invoice)
        {
            var userId = User.Identity.GetUserId();
            invoice.UserId = userId;

            if (!ModelState.IsValid)
            {
                var vm = PrepareInvoiceViewModel(invoice, userId);
                return View("Invoice", vm);
            }

            if (invoice.Id == 0)
                _invoiceRepository.Add(invoice);
            else
                _invoiceRepository.Update(invoice);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult InvoicePosition(InvoicePosition invoicePosition)
        {

            var userId = User.Identity.GetUserId();

            var product = _productRepository.GetProduct(invoicePosition.ProductId);

            if (!ModelState.IsValid)
            {
                var vm = PrepareInvoicePositionVm(invoicePosition);
                return View("InvoicePosition", vm);
            }

            invoicePosition.Value = invoicePosition.Quantity * product.Value;


            if (invoicePosition.Id == 0)
                _invoiceRepository.AddPosition(invoicePosition, userId);
            else
                _invoiceRepository.UpdatePosition(invoicePosition, userId);

            _invoiceRepository.UpdateInvoiceValue(invoicePosition.InvoiceId, userId);


            return RedirectToAction("Invoice", new { id = invoicePosition.InvoiceId });
        }

        [HttpPost]

        public ActionResult InvoicePositionUpdate(InvoicePosition invoicePosition)
        {

            var userId = User.Identity.GetUserId();

            var product = _productRepository.GetProduct(invoicePosition.ProductId);

            if (!ModelState.IsValid)
            {
                var vm = PrepareInvoicePositionVm(invoicePosition);
                return View("InvoicePosition", vm);
            }

            invoicePosition.Value = invoicePosition.Quantity * product.Value;



            _invoiceRepository.UpdatePosition(invoicePosition, userId);

            _invoiceRepository.UpdateInvoiceValue(invoicePosition.InvoiceId, userId);


            return RedirectToAction("Invoice", new { id = invoicePosition.InvoiceId });
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            var invoices = _invoiceRepository.GetInvoices(userId);
            var lastinvoiceId = invoices.Last().Id;

            if (lastinvoiceId == id)
            {

                try
                {

                    _invoiceRepository.Delete(id, userId);
                }

                catch (Exception exception)
                {
                    // logowanie
                    return Json(new { Success = false, Message = exception.Message });
                }

                return Json(new { Success = true });
            }


            return Json(new { Success = false, Message = "Można usunąć tylko ostatnią pozycję!" });
        }



        [HttpPost]
        public ActionResult DeletePosition(int id, int invoiceId)
        {
            var invoiceValue = 0m;

            try
            {
                var userId = User.Identity.GetUserId();

                _invoiceRepository.DeletePosition(id, userId);

                invoiceValue = _invoiceRepository.UpdateInvoiceValue(invoiceId, userId);


            }
            catch (Exception exception)
            {
                // logowanie
                return Json(new { Success = false, Message = exception.Message });
            }


            return Json(new { Success = true, InvoiceValue = invoiceValue });


        }


        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
                     
            return View();
        }

       
        [AllowAnonymous]
        
        public ActionResult Contact()
        {
           
            ViewBag.Message = "Your contact page. ";

            return View();
        }
 
    }

}