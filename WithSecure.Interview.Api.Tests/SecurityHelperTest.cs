using FluentAssertions;
using WithSecure.Interview.Api.Helper;

namespace WithSecure.Interview.Api.Tests.UnitTests
{
    public class SecurityHelperTest
    {
        [Fact]
        public void CalculateSHA1_WhenPassNull_ThenReturnException()
        {
            //Arrange
            var array = new byte[0];
            //Act
            Action action = () => SecurityHelper.CalculateSHA1(array);
            //Assert
            action.Should().Throw<Exception>().WithMessage("ByteArray for calculate SHA1 can not be null."); ;
        }
        [Fact]
        public void CalculateSHA1_WhenPassATargetValue_ThenReturnExpectedValue()
        {
            //Arrange
            var array = new byte[5] { 1, 2, 3, 4, 5 };
            //Act
            var result = SecurityHelper.CalculateSHA1(array);
            //Assert
            var resultArray = new byte[20] { 17, 150, 106, 185, 192, 153, 248, 250, 190, 250, 197, 76, 8, 213, 190, 43, 216, 201, 3, 175 };
            result.Should().BeEquivalentTo(resultArray);
        }
    }
}