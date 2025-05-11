using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.interfaces
{
    //ממשק המבצע את השליפות 
    // get update ....
    public interface Irepository<T>
    {
        Task<T> Getbyid(int id);
        Task< List<T>> GetAll();
        Task<T> AddItem(T item);
        Task<T> DeleteItem(int id);
        Task<T> UpDateItem(int id, T item);



    }
}
