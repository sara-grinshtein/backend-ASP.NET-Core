using Repository.Entites;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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

    [Required]
    public string description { get; set; }

    public bool hasResponse { get; set; }
    public bool? ConfirmArrival { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    [Required]
    public DateTime date { get; set; } = DateTime.Now;

    // 🆕 שדות חדשים
    public string? title { get; set; }
    public string? location { get; set; }
    public string? phone { get; set; }
}
