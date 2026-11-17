using System;

namespace AmusementParkManagement.Models
{
    public class Customer : Person
    {
        private double _heightCm;
        private int _birthYear;
        private decimal _balance;

        public double HeightCm
        {
            get => _heightCm;
            set => _heightCm = value > 0 ? value : throw new ArgumentException("Chiều cao phải lớn hơn 0!");
        }

        public int BirthYear
        {
            get => _birthYear;
            set => _birthYear = value > 1900 ? value : throw new ArgumentException("Năm sinh không hợp lệ!");
        }

        public decimal Balance
        {
            get => _balance;
            private set => _balance = value >= 0 ? value : throw new InvalidOperationException("Số dư không thể âm!");
        }

        public Customer(string id, string fullName, string phoneNumber, double heightCm, int birthYear, decimal initialBalance)
            : base(id, fullName, phoneNumber)
        {
            HeightCm = heightCm;
            BirthYear = birthYear;
            _balance = initialBalance >= 0 ? initialBalance : 0;
        }

        public void TopUp(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Số tiền nạp phải lớn hơn 0!");
            _balance += amount;
        }

        public void Deduct(decimal amount)
        {
            if (amount > _balance) throw new InvalidOperationException("Số dư tài khoản không đủ để thanh toán!");
            _balance -= amount;
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"   [Khách hàng] Chiều cao: {HeightCm} cm | Năm sinh: {BirthYear} | Số dư: {Balance:N0} VNĐ");
        }
    }
}