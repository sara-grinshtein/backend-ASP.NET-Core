using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entites
{
    public class My_areas_of_knowledge
    {
        [Key]
        public int ID_knowledge { get; set; }

        public string describtion { get; set; }

        public int volunteer_id { get; set; }  // Foreign key

        [ForeignKey("volunteer_id")]
        public Volunteer Volunteer { get; set; }  // Navigation property
    }
}
