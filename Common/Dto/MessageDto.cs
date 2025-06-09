using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class MessageDto
    {
        
        public int message_id { get; set; } 
       
        public int? volunteer_id { get; set; }

        [ForeignKey("helped_id")]
        public int helped_id { get; set; } 
        public bool isDone { get; set; }//האם טופל
        public string description { get; set; }
        public bool? confirmArrival { get; set; }

        public bool hasResponse { get; set; }
    }
}
