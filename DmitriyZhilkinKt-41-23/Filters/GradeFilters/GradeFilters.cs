namespace DmitriyZhilkinKt_41_23.Filters.GradeFilters
{
    /// Средний балл по предмету в группе.
    public class GroupDisciplineAverageFilter
    {
        public int GroupId { get; set; }
        public int DisciplineId { get; set; }
    }

    /// Оценки конкретного студента (по всем или по одной дисциплине).
    public class StudentGradesFilter
    {
        public int StudentId { get; set; }
        public int? DisciplineId { get; set; }
    }

    /// Средний балл по году обучения (курсу).
    public class YearAverageFilter
    {
        public int Course { get; set; }
    }

    /// Студенты специальности, у которых есть двойка.
    public class SpecialtyStudentsFilter
    {
        public int SpecialtyId { get; set; }
    }
}
