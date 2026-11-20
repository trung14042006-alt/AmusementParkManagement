using System;
using System.Collections.Generic;
using System.Linq;
using AmusementParkManagement.DAL;
using AmusementParkManagement.Models;

namespace AmusementParkManagement.BLL
{
    public class InvoiceService
    {
        private readonly ParkDataContext _context = ParkDataContext.Instance;

        public Invoice CreateAndPayInvoice(Customer customer, string staffId, List<Ticket> tickets)
        {
            if (tickets == null || tickets.Count == 0)
                throw new ArgumentException("Hóa đơn phải có ít nhất 01 vé!");

            string invoiceId = $"INV{DateTime.Now:yyyyMMddHHmmss}";
            var invoice = new Invoice(invoiceId, staffId);

            foreach (var ticket in tickets)
            {
                invoice.AddTicket(ticket); // BR-04: cộng dồn TotalAmount qua phương thức
            }

            // BR-02: trừ tiền trong thẻ thành viên (ném InvalidOperationException nếu thiếu số dư)
            customer.Deduct(invoice.TotalAmount);

            _context.Tickets.AddRange(tickets);
            _context.Invoices.Add(invoice);
            _context.SaveAllData();

            return invoice;
        }

        public List<Invoice> GetAllInvoices()
        {
            return _context.Invoices;
        }

        public Invoice GetInvoiceById(string invoiceId)
        {
            return _context.Invoices.FirstOrDefault(i => i.InvoiceId.Equals(invoiceId, StringComparison.OrdinalIgnoreCase))
                ?? throw new KeyNotFoundException($"Không tìm thấy hóa đơn có mã '{invoiceId}'!");
        }
    }
}