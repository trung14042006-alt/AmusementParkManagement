using System;
using System.Collections.Generic;

namespace AmusementParkManagement.Models
{
    public class Invoice
    {
        public string InvoiceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string StaffId { get; set; }
        public List<Ticket> Tickets { get; set; }
        public decimal TotalAmount { get; private set; }

        public Invoice(string invoiceId, string staffId)
        {
            InvoiceId = invoiceId;
            StaffId = staffId;
            CreatedAt = DateTime.Now;
            Tickets = new List<Ticket>();
            TotalAmount = 0;
        }

        public void AddTicket(Ticket ticket)
        {
            Tickets.Add(ticket);
            TotalAmount += ticket.CalculateFinalPrice();
        }

        public void DisplayInvoice()
        {
            Console.WriteLine($"\n================ HÓA ĐƠN THANH TOÁN ================");
            Console.WriteLine($"Mã HĐ: {InvoiceId} | Nhân viên: {StaffId} | Thời gian: {CreatedAt:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Danh sách vé ({Tickets.Count} vé):");
            foreach (var t in Tickets)
            {
                t.DisplayTicket();
            }
            Console.WriteLine($"TỔNG CỘNG: {TotalAmount:N0} VNĐ");
            Console.WriteLine($"====================================================\n");
        }
    }
}