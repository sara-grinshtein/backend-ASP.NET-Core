using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entites
{
    public class My_areas_of_knowledge
    {
        [Key]
        public int Id { get; set; } // 🔄 מזהה ייחודי לכל שורה בטבלה (לא לזהות תחום עצמו)

        // 🔗 קישור ל־Volunteer
        public int volunteer_id { get; set; }

        [ForeignKey("volunteer_id")]
        public Volunteer Volunteer { get; set; }

        // 🔗 קישור ל־KnowledgeCategory
        public int ID_knowledge { get; set; }

        [ForeignKey("ID_knowledge")]
        public KnowledgeCategory KnowledgeCategory { get; set; }
    }
}
