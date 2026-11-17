namespace DmitriyZhilkinKt_41_23.Filters.SpecialtyFilters
{
    public class SpecialtyFilter
    {
        /// <summary>Часть названия (регистр не учитывается). Пусто - без фильтра.</summary>
        public string? Title { get; set; }

        /// <summary>Часть кода специальности. Пусто - без фильтра.</summary>
        public string? Code { get; set; }
    }
}
