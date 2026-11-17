using DmitriyZhilkinKt_41_23.Models;

namespace DmitriyZhilkinKt_41_23.Dto
{
    public class DisciplineRequest
    {
        public string Name { get; set; } = string.Empty;
        public DisciplineDirection Direction { get; set; }
    }

    public class DisciplineResponse
    {
        public int DisciplineId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DisciplineDirection Direction { get; set; }
        public bool IsDeleted { get; set; }
    }
}
