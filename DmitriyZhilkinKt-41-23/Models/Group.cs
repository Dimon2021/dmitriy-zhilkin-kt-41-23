namespace DmitriyZhilkinKt_41_23.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public int Course {  get; set; }
        public int SpecialityId { get; set; }
        public bool isDeleted { get; set; }
        public Speciality Speciality { get; set; }
        public ICollection<Student> Students { get; set; }

    }
}
