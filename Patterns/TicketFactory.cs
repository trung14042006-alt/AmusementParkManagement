using System;
using AmusementParkManagement.Models;

namespace AmusementParkManagement.Patterns
{
    // Lớp trừu tượng (Creator) cho Factory Method Pattern
    public abstract class TicketFactory
    {
        public abstract Ticket CreateTicket(string ticketId, decimal basePrice, string customerId);
    }

    // Concrete Creator cho Vé Lượt
    public class SingleTicketFactory : TicketFactory
    {
        private readonly string _attractionId;

        public SingleTicketFactory(string attractionId)
        {
            _attractionId = attractionId;
        }

        public override Ticket CreateTicket(string ticketId, decimal basePrice, string customerId)
        {
            return new SingleTicket(ticketId, basePrice, customerId, _attractionId);
        }
    }

    // Concrete Creator cho Vé Trọn Gói
    public class PackageTicketFactory : TicketFactory
    {
        private readonly bool _isChildTicket;
        private readonly double _discountRate;

        public PackageTicketFactory(bool isChildTicket, double discountRate = 0.2)
        {
            _isChildTicket = isChildTicket;
            _discountRate = discountRate;
        }

        public override Ticket CreateTicket(string ticketId, decimal basePrice, string customerId)
        {
            return new PackageTicket(ticketId, basePrice, customerId, _isChildTicket, _discountRate);
        }
    }
}