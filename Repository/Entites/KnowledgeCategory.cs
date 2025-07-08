using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entites
{
    public class KnowledgeCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ✅ זה מה שיגרום ל־ID להיות אוטומטי
        public int ID_knowledge { get; set; }

        [Required]
        public string describtion { get; set; }

        public List<My_areas_of_knowledge> Volunteers { get; set; } = new();
    }
}
