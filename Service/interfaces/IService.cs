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
        T Getbyid(int id);
        List<T> GetAll();
        T AddItem(T item);
        void DeleteItem(int id);
        void UpDateItem(int id, T item);
    }
}
