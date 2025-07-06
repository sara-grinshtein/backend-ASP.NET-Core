using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entites
{
    public class KnowledgeCategory
    {
        [Key]
        public int ID_knowledge { get; set; }

        [Required]
        public string describtion { get; set; }

        public List<My_areas_of_knowledge> Volunteers { get; set; } = new();
    }
}
