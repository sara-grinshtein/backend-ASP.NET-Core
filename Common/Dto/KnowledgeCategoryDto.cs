namespace Common.Dto
{
    public class KnowledgeCategoryDto
    {
        public int ID_knowledge { get; set; }       // חייב להתאים לשם ב-Entity כדי שהמיפוי יעבוד אוטומטית (או ידני)
        public string describtion { get; set; }
    }
}
