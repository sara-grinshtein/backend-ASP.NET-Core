using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entites
{
    public class My_areas_of_knowledge
    {
        [Key]
        public int ID_knowledge { get; set; }
        public string describtion { get; set; } 
    }
}
