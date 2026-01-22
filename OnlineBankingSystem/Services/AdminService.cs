using OnlineBankingSystem.Models;
using OnlineBankingSystem.Repository;
using System.Collections.Generic;
using System.Linq;

namespace OnlineBankingSystem.Services
{
    public class AdminService
    {
        private readonly BankingDBEntities db = new BankingDBEntities();
        private readonly AccountRepository accountRepository = new AccountRepository();

        // Admin Login
        public Admin Login(string email, string password)
        {
            return db.Admins
                     .FirstOrDefault(a => a.Email_Id == email && a.Password == password);
        }

        // Pending Account Requests
        public List<AccountRequest> GetPendingRequests()
        {
            return db.AccountRequests
                     .Where(r => r.Status == "PENDING")
                     .ToList();
        }

        // View Single Request
        public AccountRequest GetRequestDetails(int id)
        {
            return db.AccountRequests.Find(id);
        }

        // Approve Request
        public void ApproveRequest(int serviceReferenceNumber)
        {
            accountRepository.ApproveAccount(serviceReferenceNumber);
        }

        // Reject Request
        public void RejectRequest(int serviceReferenceNumber)
        {
            accountRepository.RejectAccount(serviceReferenceNumber);
        }

        // View All Customers
        public List<Customer> GetAllCustomers()
        {
            return db.Customers.Include("Accounts").ToList();
        }

        // View Customer Details
        public Customer GetCustomerDetails(int customerId)
        {
            return db.Customers
                     .Include("Accounts")
                     .FirstOrDefault(c => c.Customer_Id == customerId);
        }
    }
}
