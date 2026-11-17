using System.Text.RegularExpressions;

namespace DmitriyZhilkinKt_41_23.Validators
{
    
    /// Проверяет название группы на соответствие шаблону "КТ-31-20":
    /// 2-5 букв (рус/лат), дефис, 2 цифры, дефис, 2 цифры.
    
    public static class GroupNameValidator
    {
        private static readonly Regex Pattern =
            new(@"^[A-Za-zА-Яа-яЁё]{2,5}-[0-9]{2}-[0-9]{2}$", RegexOptions.Compiled);

        public static bool IsValid(string? groupName)
        {
            return !string.IsNullOrWhiteSpace(groupName) && Pattern.IsMatch(groupName);
        }
    }
}
