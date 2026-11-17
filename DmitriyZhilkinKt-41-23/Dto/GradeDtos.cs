namespace DmitriyZhilkinKt_41_23.Dto
{
    public class GradeRequest
    {
        public int StudentId { get; set; }
        public int DisciplineId { get; set; }
        public int Value { get; set; }
    }

    public class GradeUpdateRequest
    {
        public int Value { get; set; }
    }

    public class GradeResponse
    {
        public int GradeId { get; set; }
        public int Value { get; set; }
        public int StudentId { get; set; }
        public int DisciplineId { get; set; }
        public string DisciplineName { get; set; } = string.Empty;
    }

    public class AverageResponse
    {
        
        public double? Average { get; set; }
        public int GradesCount { get; set; }
    }
}
