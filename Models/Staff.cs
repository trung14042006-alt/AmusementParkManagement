using System;

namespace AmusementParkManagement.Models
{
    public enum StaffRole { Cashier, Operator, Manager }

    public class Staff : Person
    {
        public StaffRole Role { get; set; }
        public string Shift { get; set; } // Ví dụ: "Sáng", "Chiều"

        public Staff(string id, string fullName, string phoneNumber, StaffRole role, string shift)
            : base(id, fullName, phoneNumber)
        {
            Role = role;
            Shift = shift;
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"   [Nhân viên] Chức vụ: {Role} | Ca làm: {Shift}");
        }
    }
}