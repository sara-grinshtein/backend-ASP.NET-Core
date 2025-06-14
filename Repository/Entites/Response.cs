using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entites
{
    public class Response
    {
        [Key]
        public int response_id { get; set; }

        [ForeignKey("message_id")]
        public int message_id { get; set; }

        [ForeignKey("helped_id")]
        public int helped_id { get; set; }
        public string context { get; set; }
        public int rating { get; set; }
        public virtual Message Message { get; set; }
        public virtual Helped Helped { get; set; }
        public bool isPublic { get; set; }  


        public bool hasResponse { get; set; }

    }
}
