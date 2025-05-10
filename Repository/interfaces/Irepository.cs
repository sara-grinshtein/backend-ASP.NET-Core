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
        T Getbyid(int id);
        List<T> GetAll();
        T AddItem(T item);
        T DeleteItem(int id);
        T UpDateItem(int id, T item);



    }
}
