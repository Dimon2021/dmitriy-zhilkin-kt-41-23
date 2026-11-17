namespace DmitriyZhilkinKt_41_23.Models
{
    public class Specialty
    {
        public int SpecialtyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        public ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}
