using System;

namespace AmusementParkManagement.Models
{
    public class SingleTicket : Ticket
    {
        public string AttractionId { get; set; }
        public bool IsUsed { get; set; }

        public SingleTicket(string ticketId, decimal basePrice, string customerId, string attractionId)
            : base(ticketId, basePrice, customerId)
        {
            AttractionId = attractionId;
            IsUsed = false;
        }

        public override decimal CalculateFinalPrice()
        {
            return BasePrice; // Vé lượt giữ nguyên giá gốc
        }

        public override bool IsValid()
        {
            return !IsUsed && IssueDate.Date == DateTime.Now.Date;
        }

        public override void DisplayTicket()
        {
            Console.Write("[Vé Lượt] ");
            base.DisplayTicket();
            Console.WriteLine($"   Trò chơi áp dụng: {AttractionId} | Đã dùng: {(IsUsed ? "Rồi" : "Chưa")}");
        }
    }
}