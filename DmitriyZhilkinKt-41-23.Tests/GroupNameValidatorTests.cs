using DmitriyZhilkinKt_41_23.Validators;

namespace DmitriyZhilkinKt_41_23.Tests
{
    
    public class GroupNameValidatorTests
    {
        [Theory]
        [InlineData("KT-31-20")]
        [InlineData("kt-41-23")]
        [InlineData("КТ-31-20")]
        [InlineData("ABCDE-11-99")]
        public void IsValid_CorrectGroupNames_ReturnsTrue(string groupName)
        {
            // Arrange - тестовые данные приходят из InlineData

            // Act
            var result = GroupNameValidator.IsValid(groupName);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("KT3120")]
        [InlineData("KT-31-2")]
        [InlineData("KT-31-200")]
        [InlineData("K-31-20")]
        [InlineData("KT-3A-20")]
        [InlineData(" KT-31-20")]
        [InlineData("KT-31-20 ")]
        public void IsValid_IncorrectGroupNames_ReturnsFalse(string groupName)
        {
            // Arrange - тестовые данные приходят из InlineData

            // Act
            var result = GroupNameValidator.IsValid(groupName);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsValid_NullOrEmpty_ReturnsFalse(string? groupName)
        {
            // Act
            var result = GroupNameValidator.IsValid(groupName);

            // Assert
            Assert.False(result);
        }
    }
}
