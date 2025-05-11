using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.interfaces
{
    public interface IService<T>
    {
        // קוראת לריפוזירטורי שיביא את הנתנוים וכאן רושמים את הלוגיקה //
        //לוגיקה עסקית
        Task<T> Getbyid(int id);
        Task<List<T>> GetAll();
        Task <T> AddItem(T item);
        Task  DeleteItem(int id);
        Task UpDateItem(int id, T item);
    }
}
