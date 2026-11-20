using System;
using System.Collections.Generic;
using System.Linq;
using AmusementParkManagement.DAL;
using AmusementParkManagement.Models;

namespace AmusementParkManagement.BLL
{
    public class CustomerService
    {
        private readonly ParkDataContext _context = ParkDataContext.Instance;

        public Customer RegisterCustomer(string fullName, string phoneNumber, double heightCm, int birthYear, decimal initialBalance)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Họ tên khách hàng không được để trống!");

            string newId = $"KH{(_context.Customers.Count + 1):D3}";
            var customer = new Customer(newId, fullName.Trim(), phoneNumber.Trim(), heightCm, birthYear, initialBalance);

            _context.Customers.Add(customer);
            _context.SaveAllData();
            return customer;
        }

        public void TopUpBalance(string customerId, decimal amount)
        {
            var customer = GetCustomerById(customerId);
            customer.TopUp(amount); // BR-02: kiểm tra amount > 0
            _context.SaveAllData();
        }

        public Customer GetCustomerById(string customerId)
        {
            return _context.Customers.FirstOrDefault(c => c.Id.Equals(customerId, StringComparison.OrdinalIgnoreCase))
                ?? throw new KeyNotFoundException($"Không tìm thấy khách hàng có mã '{customerId}'!");
        }

        public List<Customer> GetAllCustomers()
        {
            return _context.Customers;
        }
    }
}