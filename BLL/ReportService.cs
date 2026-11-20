using System;
using System.Linq;
using AmusementParkManagement.DAL;

namespace AmusementParkManagement.BLL
{
    public class ReportService
    {
        private readonly ParkDataContext _context = ParkDataContext.Instance;

        public void PrintDailyRevenueReport(DateTime date)
        {
            var dailyInvoices = _context.Invoices
                .Where(inv => inv.CreatedAt.Date == date.Date)
                .ToList();

            decimal totalRevenue = dailyInvoices.Sum(inv => inv.TotalAmount);
            int totalTickets = dailyInvoices.Sum(inv => inv.Tickets.Count);

            Console.WriteLine($"\n--- BÁO CÁO DOANH THU NGÀY {date:dd/MM/yyyy} ---");
            Console.WriteLine($"Tổng số hóa đơn : {dailyInvoices.Count}");
            Console.WriteLine($"Tổng số vé bán   : {totalTickets}");
            Console.WriteLine($"Tổng doanh thu   : {totalRevenue:N0} VNĐ");
            Console.WriteLine("-------------------------------------------\n");
        }

        public void PrintMonthlyRevenueReport(int month, int year)
        {
            var monthlyInvoices = _context.Invoices
                .Where(inv => inv.CreatedAt.Month == month && inv.CreatedAt.Year == year)
                .ToList();

            decimal totalRevenue = monthlyInvoices.Sum(inv => inv.TotalAmount);
            int totalTickets = monthlyInvoices.Sum(inv => inv.Tickets.Count);

            Console.WriteLine($"\n--- BÁO CÁO DOANH THU THÁNG {month:D2}/{year} ---");
            Console.WriteLine($"Tổng số hóa đơn : {monthlyInvoices.Count}");
            Console.WriteLine($"Tổng số vé bán   : {totalTickets}");
            Console.WriteLine($"Tổng doanh thu   : {totalRevenue:N0} VNĐ");
            Console.WriteLine("---------------------------------------------\n");
        }
    }
}