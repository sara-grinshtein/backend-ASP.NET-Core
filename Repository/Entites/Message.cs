
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entites
{
    public class Message
    {
        [Key]
        public int message_id { get; set; }

        public int? volunteer_id { get; set; }

        [ForeignKey("volunteer_id")]
        public virtual Volunteer Volunteer { get; set; }

        public int helped_id { get; set; }

        [ForeignKey("helped_id")]
        public virtual Helped Helped { get; set; }

        public bool isDone { get; set; }

        public string description { get; set; }

        public bool? ConfirmArrival { get; set; }
    }
}
