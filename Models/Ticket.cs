using System;

namespace AmusementParkManagement.Models
{
    public abstract class Ticket
    {
        public string TicketId { get; set; }
        public decimal BasePrice { get; set; }
        public DateTime IssueDate { get; set; }
        public string CustomerId { get; set; }

        protected Ticket(string ticketId, decimal basePrice, string customerId)
        {
            TicketId = ticketId;
            BasePrice = basePrice;
            IssueDate = DateTime.Now;
            CustomerId = customerId;
        }

        // Phương thức trừu tượng để đa hình hóa việc tính giá và kiểm tra hợp lệ
        public abstract decimal CalculateFinalPrice();
        public abstract bool IsValid();

        public virtual void DisplayTicket()
        {
            Console.WriteLine($"Mã vé: {TicketId} | Giá gốc: {BasePrice:N0} VNĐ | Ngày xuất: {IssueDate:dd/MM/yyyy HH:mm}");
        }
    }
}