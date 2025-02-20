using Bogus;
using Devflix.Admin.Domain.Exceptions;
using Devflix.Admin.Domain.Validator;
using FluentAssertions;

namespace Devflix.Admin.UnitTests.Domain.Validation;
public class DomainValidatorTest
{
    private Faker Faker {  get; set; } = new Faker();

    public static IEnumerable<object[]> GetValuesSmallerThanMinLength(int numberOfTests = 5)
    {
        Faker faker = new();
        Random random = new();

        for (int i = 0; i < numberOfTests; i++)
        {
            string value = faker.Commerce.ProductName();
            int minLength = value.Length + random.Next(1, 20);
            yield return new object[] { value, minLength };
        }
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMinLength(int numberOfTests = 5)
    {
        Faker faker = new();
        Random random = new();

        for (int i = 0; i < numberOfTests; i++)
        {
            string value = faker.Commerce.ProductName();
            int minLength = random.Next(1, (value.Length - 1));
            yield return new object[] { value, minLength };
        }
    }

    [Fact(DisplayName = nameof(ValidateShouldPassWhenValueIsNotNull))]
    [Trait("Domain", "DomainValidation - Validators")]
    public void ValidateShouldPassWhenValueIsNotNull()
    {
        var value = Faker.Commerce.Categories(1)[0];

        Action action = () => DomainValidator.NotNull(value, "FieldName");

        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(ValidateShouldThrowExceptionWhenValueIsNull))]
    [Trait("Domain", "DomainValidation - Validators")]
    public void ValidateShouldThrowExceptionWhenValueIsNull()
    {
        string? value = null;
        string fieldName = Faker.Database.Column();

        Action action = () => DomainValidator.NotNull(value, fieldName);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage($"{fieldName} should not be null");
    }

    [Fact(DisplayName = nameof(ValidateShouldPassWhenStringIsNotNullOrEmpty))]
    [Trait("Domain", "DomainValidation - Validators")]
    public void ValidateShouldPassWhenStringIsNotNullOrEmpty()
    {
        string value = Faker.Commerce.Categories(1)[0];
        string fieldName = Faker.Database.Column();

        Action action = () => DomainValidator.NotNullOrEmpty(value, fieldName);

        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(ValidateShouldThrowExceptionWhenStringIsNullOrEmpty))]
    [Trait("Domain", "DomainValidation - Validators")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("      ")]

    public void ValidateShouldThrowExceptionWhenStringIsNullOrEmpty(string target)
    { 
        string fieldName = Faker.Database.Column();

        Action action = () => DomainValidator.NotNullOrEmpty(target, fieldName);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage($"{fieldName} should not be null or empty");
    }

    [Theory(DisplayName = nameof(ValidateShouldPassWhenValueIsEqualOrGreaterThanMinLength))]
    [Trait("Domain", "DomainValidation - Validators")]
    [MemberData(nameof(GetValuesGreaterThanMinLength), parameters: 10)]
    public void ValidateShouldPassWhenValueIsEqualOrGreaterThanMinLength(string target, int minLength)
    {
        string fieldName = Faker.Database.Column();

        Action action = () => DomainValidator.MinLength(target, minLength, fieldName);

        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(ValidateShouldThrowExceptionWhenValueIsShorterThanMinLength))]
    [Trait("Domain", "DomainValidation - Validators")]
    [MemberData(nameof(GetValuesSmallerThanMinLength), parameters: 10)]
    public void ValidateShouldThrowExceptionWhenValueIsShorterThanMinLength(string target, int minLength)
    {
        string fieldName = Faker.Database.Column();

        Action action = () => DomainValidator.MinLength(target, minLength, fieldName);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage($"{fieldName} should be at leats {minLength} characters long");
    }
    // Validar tamanho max
}
