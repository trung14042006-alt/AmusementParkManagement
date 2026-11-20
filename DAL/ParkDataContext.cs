using System.Collections.Generic;
using AmusementParkManagement.Models;

namespace AmusementParkManagement.DAL
{
    public sealed class ParkDataContext
    {
        private static ParkDataContext _instance = null;
        private static readonly object _lock = new object();

        // Các Repository quản lý dữ liệu cho từng thực thể
        public IRepository<Customer> CustomerRepo { get; }
        public IRepository<Staff> StaffRepo { get; }
        public IRepository<Attraction> AttractionRepo { get; }
        public IRepository<Ticket> TicketRepo { get; }
        public IRepository<Invoice> InvoiceRepo { get; }

        // Bộ nhớ đệm (Cache) trong RAM để truy xuất nhanh
        public List<Customer> Customers { get; set; }
        public List<Staff> Staffs { get; set; }
        public List<Attraction> Attractions { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<Invoice> Invoices { get; set; }

        // Constructor private ngăn việc khởi tạo trực tiếp (Singleton Pattern)
        private ParkDataContext()
        {
            CustomerRepo = new JsonRepository<Customer>("customers.json");
            StaffRepo = new JsonRepository<Staff>("staffs.json");
            AttractionRepo = new JsonRepository<Attraction>("attractions.json");
            TicketRepo = new JsonRepository<Ticket>("tickets.json");
            InvoiceRepo = new JsonRepository<Invoice>("invoices.json");

            LoadData();
        }

        // Thuộc tính tĩnh Instance có khóa thread-safe
        public static ParkDataContext Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ParkDataContext();
                    }
                    return _instance;
                }
            }
        }

        public void LoadData()
        {
            Customers = CustomerRepo.GetAll();
            Staffs = StaffRepo.GetAll();
            Attractions = AttractionRepo.GetAll();
            Tickets = TicketRepo.GetAll();
            Invoices = InvoiceRepo.GetAll();
        }

        public void SaveAllData()
        {
            CustomerRepo.SaveAll(Customers);
            StaffRepo.SaveAll(Staffs);
            AttractionRepo.SaveAll(Attractions);
            TicketRepo.SaveAll(Tickets);
            InvoiceRepo.SaveAll(Invoices);
        }
    }
}