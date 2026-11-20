using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AmusementParkManagement.DAL
{
    public class JsonRepository<T> : IRepository<T> where T : class
    {
        private readonly string _filePath;

        public JsonRepository(string fileName)
        {
            // Đảm bảo tệp lưu trong thư mục Data/ bên cạnh file thực thi (.exe)
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            _filePath = Path.Combine(directory, fileName);
        }

        public List<T> GetAll()
        {
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }

            try
            {
                string jsonString = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<T>>(jsonString) ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi đọc file {_filePath}: {ex.Message}");
                return new List<T>();
            }
        }

        public void SaveAll(List<T> items)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(items, options);
                File.WriteAllText(_filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi ghi file {_filePath}: {ex.Message}");
            }
        }

        public void Add(T item)
        {
            var items = GetAll();
            items.Add(item);
            SaveAll(items);
        }
    }
}