using System;

namespace AmusementParkManagement.Models
{
    public class PackageTicket : Ticket
    {
        public bool IsChildTicket { get; set; }
        public double DiscountRate { get; set; } // Ví dụ: 0.2 (giảm 20% cho trẻ em)

        public PackageTicket(string ticketId, decimal basePrice, string customerId, bool isChildTicket, double discountRate)
            : base(ticketId, basePrice, customerId)
        {
            IsChildTicket = isChildTicket;
            DiscountRate = discountRate;
        }

        public override decimal CalculateFinalPrice()
        {
            return IsChildTicket ? BasePrice * (decimal)(1 - DiscountRate) : BasePrice;
        }

        public override bool IsValid()
        {
            // Vé trọn gói chỉ có hiệu lực trong ngày
            return IssueDate.Date == DateTime.Now.Date;
        }

        public override void DisplayTicket()
        {
            Console.Write("[Vé Trọn Gói] ");
            base.DisplayTicket();
            Console.WriteLine($"   Loại vé: {(IsChildTicket ? "Trẻ em (Giảm giá)" : "Người lớn")} | Giá thanh toán: {CalculateFinalPrice():N0} VNĐ");
        }
    }
}