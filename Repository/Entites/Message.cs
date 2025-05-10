using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entites
{

    public class Message
    {
        [Key]
        public int message_id { get; set; }
        [ForeignKey("volunteer_id")]
        public int volunteer_id { get; set; }
        [ForeignKey("helped_id")]
        public int helped_id { get; set; }

        public bool isDone { get; set; }//האם טופל
        public string description { get; set; }



    }
}
