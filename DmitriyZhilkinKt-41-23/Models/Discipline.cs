namespace DmitriyZhilkinKt_41_23.Models
{
    public class Discipline
    {
        public int DisciplineId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DisciplineDirection Direction { get; set; } = DisciplineDirection.Technical;
        public bool IsDeleted { get; set; }

        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
