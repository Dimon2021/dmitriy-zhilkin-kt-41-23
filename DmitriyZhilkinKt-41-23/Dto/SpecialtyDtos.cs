namespace DmitriyZhilkinKt_41_23.Dto
{
    public class SpecialtyRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    public class SpecialtyResponse
    {
        public int SpecialtyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
