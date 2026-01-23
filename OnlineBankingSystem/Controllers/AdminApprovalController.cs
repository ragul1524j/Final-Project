using OnlineBankingSystem.Models;
using OnlineBankingSystem.Services;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace OnlineBankingSystem.Controllers
{
    public class AdminApprovalController : Controller
    {
        private readonly AdminService adminService = new AdminService();
        private readonly BankingDBEntities db = new BankingDBEntities();

        // Login Page
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(string email, string password)
        {
            var admin = adminService.Login(email, password);

            if (admin == null)
            {
                ViewBag.Message = "Invalid Email or Password";
                return View();
            }

            Session["AdminId"] = admin.Admin_Id;
            return RedirectToAction("Home");
        }

        public ActionResult Home()
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("AccessDenied");

            return View();
        }

        public ActionResult PendingApprovals()
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("AccessDenied");

            return View(adminService.GetPendingRequests());
        }

        public ActionResult ViewDetails(int id)
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("AccessDenied");

            return View(adminService.GetRequestDetails(id));
        }

        public ActionResult Approve(int id)
        {
            adminService.ApproveRequest(id);
            return RedirectToAction("PendingApprovals");
        }

        public ActionResult Reject(int id)
        {
            adminService.RejectRequest(id);
            return RedirectToAction("PendingApprovals");
        }

        public ActionResult GetAllCustomers()
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("AccessDenied");

            return View(adminService.GetAllCustomers());
        }

        public ActionResult CustomerDetails(int id)
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("AccessDenied");

            var customer = db.Customers
                             .Include(c => c.Accounts)
                             .FirstOrDefault(c => c.Customer_Id == id);

            if (customer == null)
                return HttpNotFound();

            var accountNo = customer.Accounts.First().Account_Number;

            var transactions = db.Transactions
                                 .Where(t => t.From_Account == accountNo
                                          || t.To_Account == accountNo)
                                 .OrderByDescending(t => t.Transaction_Date)
                                 .ToList();

            ViewBag.Customer = customer;
            ViewBag.Transactions = transactions;

            return View();
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("AdminLogin");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
