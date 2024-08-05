using DebuggingAndRefactoringTask1.Services;
using FluentAssertions;

namespace DebuggingAndRefactoringTask1.Tests.Unit
{
    public class BankingServiceTests
    {
        private readonly BankingService _sut;

        public BankingServiceTests()
        {
            _sut = new BankingService();
        }

        [Theory(Timeout = 2000)]
        [MemberData(nameof(ValidateId_ShouldReturnTrue_WhenIdIsValid_TestData))]
        public void ValidateId_ShouldReturnTrue_WhenIdIsValid(string? id)
        {
            //Act
            var result = _sut.ValidateId(id);

            //Assert
            result.Should().BeTrue();
        }

        [Fact(Timeout = 2000)]
        public void ValidateId_ShouldReturnFalse_WhenIdIsNull()
        {
            //Arrange
            string? id = null;

            //Act
            var result = _sut.ValidateId(id);

            //Assert
            result.Should().BeFalse();
        }

        [Fact(Timeout = 2000)]
        public void ValidateId_ShouldReturnFalse_WhenIdIsEmptyString()
        {
            //Arrange
            var id = string.Empty;

            //Act
            var result = _sut.ValidateId(id);

            //Assert
            result.Should().BeFalse();
        }

        [Fact(Timeout = 2000)]
        public void ValidateId_ShouldReturnFalse_WhenIdIsWhiteSpace()
        {
            //Arrange
            var id = " ";

            //Act
            var result = _sut.ValidateId(id);

            //Assert
            result.Should().BeFalse();
        }

        [Theory(Timeout = 2000)]
        [MemberData(nameof(ValidateAccountHolderName_ShouldReturnTrue_WhenAccountHolderNameIsValid_TestData))]
        public void ValidateAccountHolderName_ShouldReturnTrue_WhenAccountHolderNameIsValid(string? accountHolderName)
        {
            //Act
            var result = _sut.ValidateAccountHolderName(accountHolderName);

            //Assert
            result.Should().BeTrue();
        }

        [Fact(Timeout = 2000)]
        public void ValidateAccountHolderName_ShouldReturnFalse_WhenAccountHolderNameIsNull()
        {
            //Arrange
            string? accountHolderName = null;

            //Act
            var result = _sut.ValidateAccountHolderName(accountHolderName);

            //Assert
            result.Should().BeFalse();
        }

        [Fact(Timeout = 2000)]
        public void ValidateAccountHolderName_ShouldReturnFalse_WhenAccountHolderNameIsEmptyString()
        {
            //Arrange
            var accountHolderName = string.Empty;

            //Act
            var result = _sut.ValidateAccountHolderName(accountHolderName);

            //Assert
            result.Should().BeFalse();
        }

        [Fact(Timeout = 2000)]
        public void ValidateAccountHolderName_ShouldReturnFalse_WhenIdAccountHolderNameWhiteSpace()
        {
            //Arrange
            var accountHolderName = " ";

            //Act
            var result = _sut.ValidateAccountHolderName(accountHolderName);

            //Assert
            result.Should().BeFalse();
        }

        [Theory(Timeout = 2000)]
        [MemberData(nameof(ValidateMonetaryValue_ShouldReturnTrue_WhenMonetaryValueIsValid_TestData))]
        public void ValidateMonetaryValue_ShouldReturnTrue_WhenMonetaryValueIsValid(decimal monetaryValue)
        {
            //Act
            var result = _sut.ValidateMonetaryValue(monetaryValue);

            //Assert
            result.Should().BeTrue();
        }

        [Fact(Timeout = 2000)]
        public void ValidateMonetaryValue_ShouldReturnFalse_WhenMonetaryValueIsNegative()
        {
            //Arrange
            var monetaryValue = -1.00m;

            //Act
            var result = _sut.ValidateMonetaryValue(monetaryValue);

            //Assert
            result.Should().BeFalse();
        }

        [Fact(Timeout = 2000)]
        public void EnsureAccountHasSufficientBalance_ShouldReturnTrue_WhenAccountHasSufficientBalance()
        {
            //Arrange
            var balance = 100.00m;
            var withdrawAmount = 50.00m;

            //Act
            var result = _sut.EnsureAccountHasSufficientBalance(balance, withdrawAmount);

            //Assert
            result.Should().BeTrue();
        }

        [Fact(Timeout = 2000)]
        public void EnsureAccountHasSufficientBalance_ShouldReturnFalse_WhenAccountDoesNotHaveSufficientBalance()
        {
            //Arrange
            var balance = 50.00m;
            var withdrawAmount = 100.00m;

            //Act
            var result = _sut.EnsureAccountHasSufficientBalance(balance, withdrawAmount);

            //Assert
            result.Should().BeFalse();
        }

        public static IEnumerable<object[]> ValidateId_ShouldReturnTrue_WhenIdIsValid_TestData =>
            new List<object[]>
            {
                new object[] {
                    "1"
                },
                new object[] {
                    "123"
                },
                new object[] {
                    "a"
                },
                new object[] {
                    "abc"
                },
                new object[] {
                    "abc123"
                }
            };

        public static IEnumerable<object[]> ValidateAccountHolderName_ShouldReturnTrue_WhenAccountHolderNameIsValid_TestData =>
            new List<object[]>
            {
                new object[] {
                    "alex"
                },
                new object[] {
                    "joe"
                },
                new object[] {
                    "steve"
                },
                new object[] {
                    "dan"
                },
                new object[] {
                    "dave"
                }
            };

        public static IEnumerable<object[]> ValidateMonetaryValue_ShouldReturnTrue_WhenMonetaryValueIsValid_TestData =>
            new List<object[]>
            {
                new object[] {
                    0.01m
                },
                new object[] {
                    0.10m
                },
                new object[] {
                    1.00m
                },
                new object[] {
                    10.00m
                },
                new object[] {
                    100.00m
                }
            };
    }
}
