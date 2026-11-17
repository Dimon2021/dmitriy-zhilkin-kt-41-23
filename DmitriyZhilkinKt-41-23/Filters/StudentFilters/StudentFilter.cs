namespace DmitriyZhilkinKt_41_23.Filters.StudentFilters
{
    public class StudentFilter
    {
        /// <summary>Точное название группы (регистр учитывается). Пусто - без фильтра.</summary>
        public string? GroupName { get; set; }

        /// <summary>Часть имени (регистр не учитывается). Пусто - без фильтра.</summary>
        public string? FirstName { get; set; }

        /// <summary>Часть фамилии (регистр не учитывается). Пусто - без фильтра.</summary>
        public string? LastName { get; set; }

        /// <summary>true - только удалённые, false - только неудалённые, null - все.</summary>
        public bool? IsDeleted { get; set; }
    }
}
