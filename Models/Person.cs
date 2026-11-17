using System;

namespace AmusementParkManagement.Models
{
    public abstract class Person
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

        protected Person(string id, string fullName, string phoneNumber)
        {
            Id = id;
            FullName = fullName;
            PhoneNumber = phoneNumber;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"ID: {Id} | Họ tên: {FullName} | SĐT: {PhoneNumber}");
        }
    }
}