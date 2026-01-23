using OnlineBankingSystem.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OnlineBankingSystem.Repository
{
    public class AccountRepository
    {
        private readonly BankingDBEntities db = new BankingDBEntities();

      
        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

       
        public void ApproveAccount(int serviceReferenceNumber)
        {
            var request = db.AccountRequests.Find(serviceReferenceNumber);

            if (request == null || request.Status != "PENDING")
                throw new Exception("Invalid or already processed request");

         
            Customer customer = new Customer
            {
                Service_Reference_Number = request.Service_Reference_Number,
                Full_Name = request.First_Name + " " + request.Last_Name,
                Mobile_Number = request.Mobile_Number,
                Email_Id = request.Email_Id,
                Aadhar = request.Aadhar,
                Created_At = DateTime.Now
            };

            db.Customers.Add(customer);
            db.SaveChanges();

         
            Account account = new Account
            {
                Customer_Id = customer.Customer_Id,
                Account_Type = "SAVINGS",
                Balance = 1000,
                Is_Active = true,
                Created_At = DateTime.Now
            };

            db.Accounts.Add(account);
            db.SaveChanges();

            InternetBanking ib = new InternetBanking
            {
                User_Id = "USR" + customer.Customer_Id,
                Customer_Id = customer.Customer_Id,
                Account_Number = account.Account_Number,
                Login_Password = HashPassword("Login@123"),
                Transaction_Password = HashPassword("Txn@123"),
                Failed_Attempts = 0,
                Is_Locked = false
            };

            db.InternetBankings.Add(ib);

           
            request.Status = "APPROVED";
            request.Approved_At = DateTime.Now;

            db.SaveChanges();
        }

        public void RejectAccount(int serviceReferenceNumber)
        {
            var request = db.AccountRequests.Find(serviceReferenceNumber);

            if (request == null)
                throw new Exception("Request not found");

            request.Status = "REJECTED";
            request.Approved_At = DateTime.Now;

            db.SaveChanges();
        }
    }
}
