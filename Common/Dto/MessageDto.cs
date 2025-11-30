using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Dto
{
    public class MessageDto
    {
        public int message_id { get; set; }

        public int? volunteer_id { get; set; }

        [ForeignKey("helped_id")]
        public int helped_id { get; set; }

        public bool isDone { get; set; } // האם טופל
        public string description { get; set; }

        public bool? confirmArrival { get; set; }

        public bool hasResponse { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // 👇 הוספנו כאן:
        public string? title { get; set; }       // כותרת הבקשה
        public string? location { get; set; }    // מיקום כתובת
        public string? phone { get; set; }       // טלפון ליצירת קשר

        public DateTime created_at { get; set; }
    }
}
