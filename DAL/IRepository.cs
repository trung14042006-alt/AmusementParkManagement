using System.Collections.Generic;

namespace AmusementParkManagement.DAL
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        void SaveAll(List<T> items);
        void Add(T item);
    }
}