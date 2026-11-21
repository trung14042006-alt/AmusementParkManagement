using System;
using AmusementParkManagement.BLL;
using AmusementParkManagement.DAL;
using AmusementParkManagement.Models;

namespace AmusementParkManagement.Presentation
{
    public class ConsoleUI
    {
        private readonly CustomerService _customerService = new CustomerService();
        private readonly TicketService _ticketService = new TicketService();
        private readonly AttractionService _attractionService = new AttractionService();
        private readonly InvoiceService _invoiceService = new InvoiceService();
        private readonly ReportService _reportService = new ReportService();

        public void SeedInitialData()
        {
            var context = ParkDataContext.Instance;
            if (context.Customers.Count > 0 || context.Attractions.Count > 0)
                return;

            // Khởi tạo nhân viên mẫu
            context.Staffs.Add(new Staff("NV01", "Nguyễn Thu Hà", "0901000111", StaffRole.Cashier, "Sáng"));
            context.Staffs.Add(new Staff("NV02", "Trần Minh Quân", "0902000222", StaffRole.Operator, "Sáng"));
            context.Staffs.Add(new Staff("NV03", "Lê Hoàng Nam", "0903000333", StaffRole.Manager, "Hành chính"));

            // Khởi tạo trò chơi mẫu (gồm cả trò chơi đang bảo trì để test BR-05)
            context.Attractions.Add(new Attraction("AT01", "Tàu lượn siêu tốc", 140, AttractionStatus.Active));
            context.Attractions.Add(new Attraction("AT02", "Đu quay ngựa gỗ", 90, AttractionStatus.Active));
            context.Attractions.Add(new Attraction("AT03", "Tháp rơi tự do", 150, AttractionStatus.Maintenance));

            // Khởi tạo khách hàng mẫu (người lớn, trẻ em, khách ít số dư)
            context.Customers.Add(new Customer("KH001", "Nguyễn Văn An", "0912345678", 165, 2004, 500000));
            context.Customers.Add(new Customer("KH002", "Trần Bảo Ngọc", "0987654321", 115, 2018, 300000));
            context.Customers.Add(new Customer("KH003", "Phạm Quốc Bảo", "0909111222", 172, 2002, 50000));

            context.SaveAllData();
        }

        public void Run()
        {
            SeedInitialData();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("==========================================================");
                Console.WriteLine("     HỆ THỐNG QUẢN LÝ KHU VUI CHƠI GIẢI TRÍ (C# OOP)      ");
                Console.WriteLine("==========================================================");
                Console.WriteLine("1. Phân hệ Thu ngân (Cashier) - Thẻ thành viên & Bán vé");
                Console.WriteLine("2. Phân hệ Nhân viên vận hành (Operator) - Soát vé cổng");
                Console.WriteLine("3. Phân hệ Quản lý (Manager) - Trò chơi & Doanh thu");
                Console.WriteLine("0. Thoát chương trình");
                Console.WriteLine("----------------------------------------------------------");
                Console.Write("Chọn vai trò thao tác: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ShowCashierMenu();
                        break;
                    case "2":
                        ShowOperatorMenu();
                        break;
                    case "3":
                        ShowManagerMenu();
                        break;
                    case "0":
                        Console.WriteLine("Đã lưu dữ liệu. Hẹn gặp lại!");
                        return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ!");
                        Pause();
                        break;
                }
            }
        }

        // ==================== 1. MENU THU NGÂN ====================
        private void ShowCashierMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("---------------- PHÂN HỆ THU NGÂN (CASHIER) ----------------");
                Console.WriteLine("1. Đăng ký thẻ thành viên mới");
                Console.WriteLine("2. Nạp tiền vào thẻ thành viên");
                Console.WriteLine("3. Bán vé lượt (Single Ticket)");
                Console.WriteLine("4. Bán vé trọn gói (Package Ticket)");
                Console.WriteLine("5. Xem danh sách khách hàng & hóa đơn");
                Console.WriteLine("0. Quay lại menu chính");
                Console.Write("Chọn chức năng: ");

                string choice = Console.ReadLine();
                if (choice == "0") break;

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Họ và tên khách hàng: ");
                            string name = Console.ReadLine();
                            Console.Write("Số điện thoại: ");
                            string phone = Console.ReadLine();
                            double height = ReadDouble("Chiều cao (cm): ");
                            int birthYear = ReadInt("Năm sinh: ");
                            decimal initBalance = ReadDecimal("Số tiền nạp ban đầu (VNĐ): ");

                            var newCus = _customerService.RegisterCustomer(name, phone, height, birthYear, initBalance);
                            Console.WriteLine("\nĐăng ký thành công!");
                            newCus.DisplayInfo();
                            break;

                        case "2":
                            Console.Write("Nhập mã khách hàng (VD: KH001): ");
                            string topUpId = Console.ReadLine();
                            decimal amount = ReadDecimal("Số tiền cần nạp (VNĐ): ");

                            _customerService.TopUpBalance(topUpId, amount);
                            var updatedCus = _customerService.GetCustomerById(topUpId);
                            Console.WriteLine($"Nạp tiền thành công! Số dư mới: {updatedCus.Balance:N0} VNĐ");
                            break;

                        case "3":
                            ShowAttractionsList();
                            Console.Write("Nhập mã khách hàng (VD: KH001): ");
                            string cusIdSingle = Console.ReadLine();
                            Console.Write("Nhập mã trò chơi (VD: AT01): ");
                            string attId = Console.ReadLine();
                            decimal singlePrice = ReadDecimal("Giá vé lượt gốc (VD: 80000): ");

                            var invSingle = _ticketService.PurchaseSingleTicket(cusIdSingle, "NV01", attId, singlePrice);
                            Console.WriteLine("\nThanh toán vé lượt thành công!");
                            invSingle.DisplayInvoice();
                            break;

                        case "4":
                            Console.Write("Nhập mã khách hàng (VD: KH002): ");
                            string cusIdPkg = Console.ReadLine();
                            decimal pkgPrice = ReadDecimal("Giá vé trọn gói gốc (VD: 300000): ");

                            var invPkg = _ticketService.PurchasePackageTicket(cusIdPkg, "NV01", pkgPrice);
                            Console.WriteLine("\nThanh toán vé trọn gói thành công!");
                            invPkg.DisplayInvoice();
                            break;

                        case "5":
                            Console.WriteLine("\n--- DANH SÁCH KHÁCH HÀNG ---");
                            foreach (var c in _customerService.GetAllCustomers()) c.DisplayInfo();

                            Console.WriteLine("\n--- LỊCH SỬ HÓA ĐƠN ---");
                            foreach (var inv in _invoiceService.GetAllInvoices()) inv.DisplayInvoice();
                            break;

                        default:
                            Console.WriteLine("Lựa chọn không hợp lệ!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n[LỖI NGHIỆP VỤ]: {ex.Message}");
                }
                Pause();
            }
        }

        // ==================== 2. MENU VẬN HÀNH ====================
        private void ShowOperatorMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("----------- PHÂN HỆ VẬN HÀNH TRÒ CHƠI (OPERATOR) -----------");
                Console.WriteLine("1. Quẹt vé kiểm soát khách qua cổng trò chơi");
                Console.WriteLine("2. Xem danh sách vé đã phát hành & Trạng thái trò chơi");
                Console.WriteLine("0. Quay lại menu chính");
                Console.Write("Chọn chức năng: ");

                string choice = Console.ReadLine();
                if (choice == "0") break;

                try
                {
                    switch (choice)
                    {
                        case "1":
                            ShowAttractionsList();
                            Console.Write("Nhập mã vé (VD: TK...): ");
                            string ticketId = Console.ReadLine();
                            Console.Write("Nhập mã khách hàng (VD: KH001): ");
                            string customerId = Console.ReadLine();
                            Console.Write("Nhập mã trò chơi (VD: AT01): ");
                            string attractionId = Console.ReadLine();

                            Console.WriteLine();
                            _attractionService.ValidateAndAllowEntry(ticketId, customerId, attractionId);
                            break;

                        case "2":
                            ShowAttractionsList();
                            Console.WriteLine("\n--- DANH SÁCH VÉ ĐÃ PHÁT HÀNH ---");
                            foreach (var t in _ticketService.GetAllTickets()) t.DisplayTicket();
                            break;

                        default:
                            Console.WriteLine("Lựa chọn không hợp lệ!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n[LỖI KIỂM SOÁT]: {ex.Message}");
                }
                Pause();
            }
        }

        // ==================== 3. MENU QUẢN LÝ ====================
        private void ShowManagerMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("---------------- PHÂN HỆ QUẢN LÝ (MANAGER) ----------------");
                Console.WriteLine("1. Xem danh mục trò chơi");
                Console.WriteLine("2. Thêm trò chơi mới");
                Console.WriteLine("3. Cập nhật trạng thái trò chơi (Hoạt động / Bảo trì)");
                Console.WriteLine("4. Thống kê doanh thu ngày hôm nay");
                Console.WriteLine("5. Thống kê doanh thu theo tháng");
                Console.WriteLine("0. Quay lại menu chính");
                Console.Write("Chọn chức năng: ");

                string choice = Console.ReadLine();
                if (choice == "0") break;

                try
                {
                    switch (choice)
                    {
                        case "1":
                            ShowAttractionsList();
                            break;

                        case "2":
                            Console.Write("Mã trò chơi mới (VD: AT04): ");
                            string id = Console.ReadLine();
                            Console.Write("Tên trò chơi: ");
                            string name = Console.ReadLine();
                            double minHeight = ReadDouble("Chiều cao tối thiểu yêu cầu (cm): ");

                            var att = _attractionService.AddAttraction(id, name, minHeight);
                            Console.WriteLine("Đã thêm trò chơi mới thành công!");
                            att.Display();
                            break;

                        case "3":
                            ShowAttractionsList();
                            Console.Write("Nhập mã trò chơi cần đổi trạng thái: ");
                            string attId = Console.ReadLine();
                            Console.WriteLine("Chọn trạng thái mới (1: Active - Hoạt động | 2: Maintenance - Bảo trì): ");
                            string st = Console.ReadLine();
                            AttractionStatus newStatus = st == "2" ? AttractionStatus.Maintenance : AttractionStatus.Active;

                            _attractionService.UpdateStatus(attId, newStatus);
                            Console.WriteLine($"Đã cập nhật trạng thái trò chơi {attId} thành {newStatus}!");
                            break;

                        case "4":
                            _reportService.PrintDailyRevenueReport(DateTime.Now);
                            break;

                        case "5":
                            int month = ReadInt("Nhập tháng (1-12): ");
                            int year = ReadInt("Nhập năm (VD: 2026): ");
                            _reportService.PrintMonthlyRevenueReport(month, year);
                            break;

                        default:
                            Console.WriteLine("Lựa chọn không hợp lệ!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n[LỖI QUẢN TRỊ]: {ex.Message}");
                }
                Pause();
            }
        }

        // ==================== HÀM TIỆN ÍCH NHẬP LIỆU AN TOÀN ====================
        private void ShowAttractionsList()
        {
            Console.WriteLine("\n--- DANH SÁCH TRÒ CHƠI HIỆN HÀNH ---");
            foreach (var a in _attractionService.GetAllAttractions())
            {
                a.Display();
            }
            Console.WriteLine();
        }

        private int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int val)) return val;
                Console.WriteLine("Vui lòng nhập số nguyên hợp lệ!");
            }
        }

        private double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out double val)) return val;
                Console.WriteLine("Vui lòng nhập số thực hợp lệ!");
            }
        }

        private decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal val)) return val;
                Console.WriteLine("Vui lòng nhập số tiền hợp lệ!");
            }
        }

        private void Pause()
        {
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
        }
    }
}