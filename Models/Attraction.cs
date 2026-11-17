using System;

namespace AmusementParkManagement.Models
{
    public enum AttractionStatus { Active, Maintenance }

    public class Attraction
    {
        public string AttractionId { get; set; }
        public string Name { get; set; }
        public double MinHeightCm { get; set; }
        public AttractionStatus Status { get; set; }

        public Attraction(string attractionId, string name, double minHeightCm, AttractionStatus status = AttractionStatus.Active)
        {
            AttractionId = attractionId;
            Name = name;
            MinHeightCm = minHeightCm;
            Status = status;
        }

        public bool CheckCondition(Customer customer)
        {
            if (Status == AttractionStatus.Maintenance)
            {
                Console.WriteLine($"Trò chơi [{Name}] đang bảo trì!");
                return false;
            }
            if (customer.HeightCm < MinHeightCm)
            {
                Console.WriteLine($"Khách hàng {customer.FullName} không đủ chiều cao ({customer.HeightCm}cm < {MinHeightCm}cm) để chơi {Name}.");
                return false;
            }
            return true;
        }

        public void Display()
        {
            Console.WriteLine($"[Trò chơi] {AttractionId} - {Name} | Min Cao: {MinHeightCm}cm | Trạng thái: {Status}");
        }
    }
}