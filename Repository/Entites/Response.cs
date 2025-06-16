using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entites
{
    public class Response
    {
        [Key]
        public int response_id { get; set; }

        public int message_id { get; set; }
        // public int helped_id { get; set; }

        [ForeignKey(nameof(message_id))]
        public virtual Message Message { get; set; }

     //   [ForeignKey(nameof(helped_id))]
     //   public virtual Helped Helped { get; set; }

        public string context { get; set; }
        public int rating { get; set; }
        public bool isPublic { get; set; }
    }
}

