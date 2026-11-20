using System;
using System.Collections.Generic;
using System.Linq;
using AmusementParkManagement.DAL;
using AmusementParkManagement.Models;
using AmusementParkManagement.Patterns;

namespace AmusementParkManagement.BLL
{
    public class TicketService
    {
        private readonly ParkDataContext _context = ParkDataContext.Instance;
        private readonly CustomerService _customerService = new CustomerService();
        private readonly InvoiceService _invoiceService = new InvoiceService();

        public Invoice PurchaseSingleTicket(string customerId, string staffId, string attractionId, decimal basePrice)
        {
            var customer = _customerService.GetCustomerById(customerId);
            var attraction = _context.Attractions.FirstOrDefault(a => a.AttractionId.Equals(attractionId, StringComparison.OrdinalIgnoreCase))
                ?? throw new KeyNotFoundException($"Không tìm thấy trò chơi có mã '{attractionId}'!");

            if (attraction.Status == AttractionStatus.Maintenance)
                throw new InvalidOperationException($"Trò chơi '{attraction.Name}' đang bảo trì, tạm ngừng bán vé lượt!");

            TicketFactory factory = new SingleTicketFactory(attraction.AttractionId);
            string ticketId = $"TK{DateTime.Now:HHmmssfff}";
            Ticket ticket = factory.CreateTicket(ticketId, basePrice, customer.Id);

            return _invoiceService.CreateAndPayInvoice(customer, staffId, new List<Ticket> { ticket });
        }

        public Invoice PurchasePackageTicket(string customerId, string staffId, decimal basePrice, double childDiscountRate = 0.2)
        {
            var customer = _customerService.GetCustomerById(customerId);
            int age = DateTime.Now.Year - customer.BirthYear;
            bool isChild = age < 12;

            TicketFactory factory = new PackageTicketFactory(isChild, childDiscountRate);
            string ticketId = $"TK{DateTime.Now:HHmmssfff}";
            Ticket ticket = factory.CreateTicket(ticketId, basePrice, customer.Id);

            return _invoiceService.CreateAndPayInvoice(customer, staffId, new List<Ticket> { ticket });
        }

        public List<Ticket> GetAllTickets()
        {
            return _context.Tickets;
        }
    }
}