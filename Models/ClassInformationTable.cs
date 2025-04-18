namespace RAZOR_PAGE_KOPYASI.Models
{
    public class ClassInformationTable
    {
        public int Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string Instructor { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Semester { get; set; } = string.Empty;
    }
}
