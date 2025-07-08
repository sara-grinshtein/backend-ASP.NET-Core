// Controllers/KnowledgeCategoryController.cs
using Microsoft.AspNetCore.Mvc;
using Service.interfaces;
using Common.Dto;
namespace Common.Dto
{
    [ApiController]
    [Route("api/[controller]")]
    public class KnowledgeCategoryController : ControllerBase
    {
        private readonly IService<KnowledgeCategoryDto> _service;

        public KnowledgeCategoryController(IService<KnowledgeCategoryDto> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<KnowledgeCategoryDto>> Get()
        {
            var list = await _service.GetAll();
            return list
                .GroupBy(x => x.ID_knowledge)
                .Select(g => g.First()) // מסיר כפילויות
                .ToList();
        }



        [HttpPost("init")]
        public async Task<IActionResult> SeedKnowledgeCategories()
        {
            var categories = new List<KnowledgeCategoryDto>
    {
        new() { describtion = "מכונאות בסיסית" },
        new() { describtion = "זיהוי תקלות פשוטות" },
        new() { describtion = "החלפת גלגל / מצבר" },
        new() { describtion = "טיפול בתקלות סטרטר / חוטי התנעה" },
        new() { describtion = "חשמל רכב בסיסי" },
        new() { describtion = "שימוש בכבלים / בוסטר" },
        new() { describtion = "זיהוי בעיות מצבר / פיוזים" },
        new() { describtion = "פתיחת רכבים נעולים" },
        new() { describtion = "טיפול בפנצ'ר / תקר בגלגל" },
        new() { describtion = "החלפת גלגלים בבטיחות" },
        new() { describtion = "שימוש נכון בג'ק ובכלים" },
        new() { describtion = "אבטחת זירת האירוע" },
        new() { describtion = "התנעה של רכב תקוע / בלי דלק" },
        new() { describtion = "סיוע בהבאת דלק / מים לרדיאטור" },
        new() { describtion = "חילוץ רכבים מבוץ / חול" },
        new() { describtion = "שימוש ברצועות גרירה" },
        new() { describtion = "ידע בסיסי בחילוץ" },
        new() { describtion = "ניווט חכם לאזורים קשים להגעה" },
        new() { describtion = "עזרה ראשונה -כולל לילדים וקשישים)" },
        new() { describtion = "טיפול באדם שנמצא בתוך מעלית תקועה" },
        new() { describtion = "ידע בסיסי בטיפול בתקלות מעלית" },
        new() { describtion = "פריצת דלתות חוקית ובטוחה" },
        new() { describtion = "טיפול בילד אובד ברחוב" },
        new() { describtion = "חשמל רכב בסיסי" },
        new() { describtion = "דלק שמן ומים" },
          new() { describtion = "סיוע בכבלים" },
            new() { describtion = "חילוץ ופתיחת רכבים" },
              new() { describtion = "החלפת גלגל" },
                new() { describtion = "חילוץ במצבי לכידה / תקיעה של גוף באובייקט" },
                new() { describtion = "החלפת נורה / פנס רכב" },
                new() { describtion = "החלפת צמיגים" },
                new() { describtion = "החלפת נורות רכב" },
                new() { describtion = "החלפת רפידות בלמים" },
                new() { describtion = "החלפת שמן מנוע" },
                new() { describtion = "החלפת מסנן שמן" },
                new() { describtion = "החלפת מסנן אוויר" },
                new() { describtion = "החלפת מסנן דלק" },
                new() { describtion = "החלפת רצועת טיימינג" },
                new() { describtion = "החלפת רצועת מנוע" },
                  new() { describtion = "החלפת מצבר רכב" },
                 new() { describtion = "החלפת נוזל בלמים" },
                new() { describtion = "סיוע בלכידות גוף" },
        new() { describtion = "הזעקת שירותים מקצועיים וליווי" },
        new() { describtion = "מענה לנכים / אוכלוסיות מיוחדות" },
         new() { describtion = "סיוע להורים תקועים עם ילדים ברכב" },
        new() { describtion = "תחזוקה שוטפת של ציוד וחומרים" },



    };

            foreach (var category in categories)
            {
                await _service.AddItem(category);
            }

            return Ok("הרשימה נוספה בהצלחה ✅");
        }

    }
}