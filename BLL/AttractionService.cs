using System;
using System.Collections.Generic;
using System.Linq;
using AmusementParkManagement.DAL;
using AmusementParkManagement.Models;

namespace AmusementParkManagement.BLL
{
    public class AttractionService
    {
        private readonly ParkDataContext _context = ParkDataContext.Instance;

        public Attraction AddAttraction(string attractionId, string name, double minHeightCm)
        {
            if (_context.Attractions.Any(a => a.AttractionId.Equals(attractionId, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"Mã trò chơi '{attractionId}' đã tồn tại!");

            if (minHeightCm < 0)
                throw new ArgumentException("Chiều cao tối thiểu không được âm!");

            var attraction = new Attraction(attractionId.ToUpper(), name.Trim(), minHeightCm, AttractionStatus.Active);
            _context.Attractions.Add(attraction);
            _context.SaveAllData();
            return attraction;
        }

        public void UpdateStatus(string attractionId, AttractionStatus newStatus)
        {
            var attraction = GetAttractionById(attractionId);
            attraction.Status = newStatus;
            _context.SaveAllData();
        }

        public bool ValidateAndAllowEntry(string ticketId, string customerId, string attractionId)
        {
            var attraction = GetAttractionById(attractionId);
            var customer = _context.Customers.FirstOrDefault(c => c.Id.Equals(customerId, StringComparison.OrdinalIgnoreCase))
                ?? throw new KeyNotFoundException($"Không tìm thấy khách hàng '{customerId}'!");
            var ticket = _context.Tickets.FirstOrDefault(t => t.TicketId.Equals(ticketId, StringComparison.OrdinalIgnoreCase))
                ?? throw new KeyNotFoundException($"Không tìm thấy vé '{ticketId}'!");

            if (!ticket.CustomerId.Equals(customer.Id, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Từ chối: Vé không thuộc sở hữu của khách hàng này!");
                return false;
            }

            // BR-01 & BR-05: kiểm tra trạng thái bảo trì và chiều cao tối thiểu
            if (!attraction.CheckCondition(customer))
            {
                return false;
            }

            // BR-03: kiểm tra hiệu lực vé thông qua đa hình
            if (!ticket.IsValid())
            {
                Console.WriteLine("Từ chối: Vé đã hết hạn sử dụng trong ngày hoặc đã qua cổng!");
                return false;
            }

            if (ticket is SingleTicket singleTicket)
            {
                if (!singleTicket.AttractionId.Equals(attraction.AttractionId, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Từ chối: Vé lượt này chỉ áp dụng cho trò chơi '{singleTicket.AttractionId}'!");
                    return false;
                }
                singleTicket.IsUsed = true;
            }

            _context.SaveAllData();
            Console.WriteLine($"HỢP LỆ: Mời khách hàng {customer.FullName} tham gia trò chơi [{attraction.Name}]!");
            return true;
        }

        public Attraction GetAttractionById(string attractionId)
        {
            return _context.Attractions.FirstOrDefault(a => a.AttractionId.Equals(attractionId, StringComparison.OrdinalIgnoreCase))
                ?? throw new KeyNotFoundException($"Không tìm thấy trò chơi có mã '{attractionId}'!");
        }

        public List<Attraction> GetAllAttractions()
        {
            return _context.Attractions;
        }
    }
}